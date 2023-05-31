using System.Text.RegularExpressions;
using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.Explore;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Entities.Library;
using Library.DataAccess.Persistence;
using Library.DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly DatabaseContext _dbContext;
    
    public BookRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }
    
    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
        
        return book;
    }

    public async Task<Book?> GetBookWithCategoryByIdAsync(int bookId)
    {
        var book = await _dbContext.Books
            .Include(b => b.Author)
            .SingleOrDefaultAsync(b => b.Id == bookId);
        
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
        
        // Split query in order to increase performance from 500ms to 100ms !!!
        await _dbContext.Entry(book)
            .Collection(x => x.BookCategories)
            .Query()
            .Include(x => x.Category)
            .LoadAsync();
    
        return book;
    }

    public async Task<IEnumerable<Book>> GetBooksForAllBookshelves()
    {
        var bookshelves = await _dbContext.Bookshelves
            .Include(bs => bs.Categories)
            .ToListAsync();
        
        var categoryIds = bookshelves
            .SelectMany(bs => bs.Categories.Select(c => c.Id))
            .Distinct().ToList();
        
        var bookIds = _dbContext.BooksCategories
            .Where(bc => categoryIds.Contains(bc.CategoryId))
            .Select(bc => bc.BookId)
            .Distinct()
            .ToList();
        
        var books = _dbContext.Books
            .Where(b => bookIds.Contains(b.Id))
            .GroupBy(b => b.Id)
            .Select(g => g.First())
            .Take(bookshelves.Count * 10)
            .ToList();

        return books;
    }

    // aprox 300ms
    // var booksGroupedByBookshelf = await _databaseContext.BooksCategories
    //     .Include(bc => bc.Book)
    //     .Include(bc => bc.Category)
    //     .ThenInclude(c => c.Bookshelf)
    //     .GroupBy(bc => bc.Category.Bookshelf)
    //     .Select(g => new { Bookshelf = g.Key, Books = g.Select(bc => bc.Book) })
    //     .ToListAsync();
    //
    // return booksGroupedByBookshelf.ToDictionary(
    //     bgbb => bgbb.Bookshelf.Title, 
    //     bgbb => bgbb.Books.ToList());
         
         
    // aprox 250ms
         
    /*
     * In this implementation, the query for retrieving books has been simplified
     * to only include the Book and Category entities, and uses a projection to
     * select only the Title and Id of the Bookshelf entity. The AsNoTracking() method
     * is called to disable change tracking for the entities returned by the query. 
     */
    
    public async Task<Dictionary<string, BookshelfWithBooksDto>> GetBooksGroupedByBookshelf(int maxResults)
    {
        var booksGroupedByBookshelf = await _dbContext.BooksCategories
            .AsNoTracking()
            .Include(bc => bc.Book.Author)
            .Select(bc => new { BookshelfId = bc.Category.Bookshelf.Id, BookshelfTitle = bc.Category.Bookshelf.Title, Book = bc.Book })
            .ToListAsync();

        var groupedBooks = booksGroupedByBookshelf
            .GroupBy(bgbb => bgbb.BookshelfTitle)
            .ToDictionary(
                g => g.Key,
                g => new BookshelfWithBooksDto
                {
                    Id = g.First().BookshelfId,
                    TotalBooks = g.Count(),
                    Books = g.Select(bgbb => bgbb.Book)
                        .OrderBy(x => Guid.NewGuid())
                        .Take(maxResults)
                        .ToList()
                }
            );

        return groupedBooks;
    }

    public async Task<List<BookshelfCategoryWithBooksDto>> GetBooksGroupedByCategoryAndBookshelf(int maxResults)
    {
        var categoriesWithBooks = await _dbContext.Categories
            .AsNoTracking()
            .Include(c => c.Bookshelf)
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .ThenInclude(b => b.Author)
            .ToListAsync();

        var groupedCategories = categoriesWithBooks
            .GroupBy(c => new { c.Bookshelf.Id, c.Bookshelf.Title })
            .OrderBy(g => g.Key.Id)
            .Select(g => new BookshelfCategoryWithBooksDto
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                Categories = g.Select(c => new ExploreCategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    TotalBooks = c.BookCategories.Count(),
                    Books = c.BookCategories.Select(bc => bc.Book)
                        .OrderBy(x => Guid.NewGuid())
                        .Take(maxResults)
                        .ToList()
                }).ToList()
            })
            .ToList();

        return groupedCategories;
    }

    public async Task<BookWithContentDto> GetReadingBookByIdAsync(int bookId)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
    
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
    
        var bookWithContent = new BookWithContentDto
        {
            Id = book.Id
        };
    
        if (!string.IsNullOrEmpty(book.HtmlContentUrl))
        {
            var url = $"https://www.gutenberg.org{book.HtmlContentUrl}";
            
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            // extract the content between the pg-header and pg-footer sections
            var content = await BookContentUtil.GetContentBetweenSectionsAsync(html);

            // remove any images from the content
            content = new Regex("<img[^>]+>").Replace(content, "");

            bookWithContent.Content = content;
        }

        return bookWithContent;
    }
    
    public async Task<int> CountWordsInResponseAsync(int bookId)
    {
        var bookWithContent = await GetReadingBookByIdAsync(bookId);
        
        // var plainTextContent = BookContentUtil.RemoveHtmlTags(bookWithContent.Content);
        // var words = plainTextContent.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        // return words.Length;
        
        return BookContentUtil.CountWords(bookWithContent.Content);
    }
    
    public async Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int bookId)
    {
        var wordCount = await CountWordsInResponseAsync(bookId);

        return BookContentUtil.CalculateReadingTime(wordCount);
    }

    public async Task<UserReadingBooks> GetUserReadingTimeSpentAsync(int bookId, string username)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        if (book == default) throw new Exception("Invalid book id.");

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (user == default) throw new Exception("Invalid user.");
        
        var userReadingBook = await _dbContext.UserReadingBooks
            .SingleOrDefaultAsync(c => c.User.Username == username & c.BookId == bookId);

        if (userReadingBook == null) throw new Exception("User have not started to read this book yet.");
        
        return userReadingBook;
    }
    
    public async Task<bool> UpdateUserReadingTimeSpentAsync(int bookId, string username, int timeSpent)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        if (book == default) throw new Exception("Invalid book id.");

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (user == default) throw new Exception("Invalid user.");
        
        var userReadingBook = await _dbContext.UserReadingBooks
                .SingleOrDefaultAsync(c => c.User.Username == username & c.BookId == bookId);

        if (userReadingBook == null) // just started reading the book
        {
            // add into db the new book
            var newReadingBook = new UserReadingBooks
            {
                UserId = user.Id,
                BookId = book.Id,
                MinutesSpent = timeSpent,
                IsBookFinished = false
            };
            
            _dbContext.UserReadingBooks.Add(newReadingBook);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            // update the timeSpent into db
            userReadingBook.MinutesSpent = timeSpent;
            
            _dbContext.UserReadingBooks.Update(userReadingBook);
            await _dbContext.SaveChangesAsync();
        }
        
        return true;
    }

    public async Task<bool> SetFinishedReadingBookByIdAsync(int bookId, string username, int timeSpent)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        if (book == default) throw new Exception("Invalid book id.");

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (user == default) throw new Exception("Invalid user.");

        var checkExistence =  await CheckIsBookFinishedReading(bookId, username);
        if (checkExistence) throw new Exception("Already finished reading this book.");
        
        var totalTimeResponse = await GetReadingTimeOfBookContent(bookId);
        var totalMinutesConverted = TimeConvertor.ConvertResponseMinutes(totalTimeResponse);

        if (timeSpent < totalMinutesConverted) throw new Exception("Time spent reading is lower than average reading time.");

        var newCompletedBook = new UserReadingBooks
        {
            UserId = user.Id,
            BookId = book.Id,
            MinutesSpent = timeSpent,
            IsBookFinished = true,
            Timestamp = DateTime.Now.ToString("dd/MM/yyyy")
        };

        _dbContext.UserReadingBooks.Update(newCompletedBook);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckIsBookFinishedReading(int bookId, string username)
    {
        var checkIsFinished =
            await _dbContext.UserReadingBooks
                .SingleOrDefaultAsync(c => c.User.Username == username & c.BookId == bookId);
        
        return checkIsFinished is { IsBookFinished: true };
    }


    public async Task<IEnumerable<Book?>> GetTrendingNowBooksAsync()
    {
        // TODO implement
        var result = await _dbContext.Books.Take(10).Include(b => b.Author).ToListAsync();

        return result;
    }
    
    public async Task<IEnumerable<Book?>> GetTrendingWeekBooksAsync()
    {
        // TODO implement
        var result = await _dbContext.Books.Skip(100).Take(10).Include(b => b.Author).ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Book>> SearchBooksByFilterAsync(string searchByKey, int maxResults)
    {
        var filteredBooks = await _dbContext.Books
            .Where(book => book.Title.ToUpper().Contains(searchByKey.ToUpper()))
            .Take(maxResults)
            .Include(book => book.Author)
            .ToListAsync();

        return filteredBooks;
    }
}