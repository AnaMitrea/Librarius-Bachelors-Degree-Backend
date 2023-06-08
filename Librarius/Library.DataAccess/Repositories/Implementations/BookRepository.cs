using System.Text.RegularExpressions;
using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.Explore;
using Library.DataAccess.Entities.Library;
using Library.DataAccess.Entities.User;
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
    
    public async Task<Dictionary<string, BookshelfWithBooksDto>> GetBooksGroupedByBookshelf(int? maxResults, string? title)
    {
        var query = _dbContext.BooksCategories
            .AsNoTracking()
            .Include(bc => bc.Book.Author)
            .Select(bc => new { BookshelfId = bc.Category.Bookshelf.Id, BookshelfTitle = bc.Category.Bookshelf.Title, bc.Book });

        if (title != null)
        {
            query = query.Where(bc => bc.BookshelfTitle.Contains(title));
        }

        var booksGroupedByBookshelf = await query.ToListAsync();

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
                        .Take(maxResults ?? g.Count())
                        .ToList()
                }
            );

        return groupedBooks;
    }
    
    // Ordered books from A to Z -> by BOOKSHELF only
    public async Task<Dictionary<string, OrderedBookshelfWithBooksDto>> GetOrderedBooksGroupedByBookshelf(int? maxResults, string? title)
    {
        var query = _dbContext.BooksCategories
            .AsNoTracking()
            .Include(bc => bc.Book.Author)
            .Select(bc => new { BookshelfId = bc.Category.Bookshelf.Id, BookshelfTitle = bc.Category.Bookshelf.Title, bc.Book });

        if (title != null)
        {
            query = query.Where(bc => bc.BookshelfTitle.Contains(title));
        }

        var booksGroupedByBookshelf = await query.ToListAsync();

        var groupedBooks = booksGroupedByBookshelf
            .GroupBy(bgbb => bgbb.BookshelfTitle)
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key,
                g => new OrderedBookshelfWithBooksDto
                {
                    Id = g.First().BookshelfId,
                    TotalBooks = g.Count(),
                    Books = g.Where(bgbb => char.IsLetter(GetFirstValidCharacter(bgbb.Book.Title)))
                        .GroupBy(bgbb => GetFirstValidCharacter(bgbb.Book.Title).ToString().ToUpper())
                        .OrderBy(bg => bg.Key)
                        .ToDictionary(
                            bg => bg.Key,
                            bg => bg.Select(book => book.Book)
                                .OrderBy(book => book.Title)
                                .Take(maxResults ?? int.MaxValue)
                                .ToList()
                        )
                }
            );

        return groupedBooks;
    }

    
    public async Task<Dictionary<string, BookshelfWithBooksDto>> GetGroupedBookshelves(string? title)
    {
        var query = _dbContext.BooksCategories
            .AsNoTracking()
            .Select(bc => new { BookshelfId = bc.Category.Bookshelf.Id, BookshelfTitle = bc.Category.Bookshelf.Title});

        if (title != null)
        {
            query = query.Where(bc => bc.BookshelfTitle.Contains(title));
        }

        var booksGroupedByBookshelf = await query.ToListAsync();

        var groupedBooks = booksGroupedByBookshelf
            .GroupBy(bgbb => bgbb.BookshelfTitle)
            .ToDictionary(
                g => g.Key,
                g => new BookshelfWithBooksDto
                {
                    Id = g.First().BookshelfId,
                    TotalBooks = g.Count()
                }
            );

        return groupedBooks;
    }

    public async Task<List<BookshelfCategoryWithBooksDto>> GetBooksGroupedByCategoryAndBookshelf(int? maxResults, string? title)
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
                    Books = maxResults is > 0
                        
                        ? c.BookCategories.Select(bc => bc.Book)
                            .OrderBy(x => Guid.NewGuid())
                            .Take(maxResults.Value)
                            .ToList()
                        
                        : c.BookCategories.Select(bc => bc.Book)
                            .OrderBy(x => Guid.NewGuid())
                            .ToList()
                        
                }).ToList()
            })
            .ToList();

        return groupedCategories;
    }
    
    // Categories by Bookshelves NO BOOKS
    public async Task<List<BookshelfCategoryWithBooksDto>> GetGroupedCategoryAndBookshelf(string? title)
    {
        var query = _dbContext.Categories
            .Select(c => new
            {
                CategoryId = c.Id,
                CategoryTitle = c.Title,
                BookshelfId = c.Bookshelf.Id,
                BookshelfTitle = c.Bookshelf.Title,
                TotalBooks = c.BookCategories.Count()
            });

        if (title != null)
        {
            query = query.Where(c => c.CategoryTitle.Contains(title));
        }

        var categoriesWithBooks = await query.ToListAsync();

        var groupedCategories = categoriesWithBooks
            .GroupBy(c => new { c.BookshelfId, c.BookshelfTitle })
            .OrderBy(g => g.Key.BookshelfId)
            .Select(g => new BookshelfCategoryWithBooksDto
            {
                Id = g.Key.BookshelfId,
                Title = g.Key.BookshelfTitle,
                Categories = g.Select(c => new ExploreCategoryDto
                {
                    Id = c.CategoryId,
                    Title = c.CategoryTitle,
                    TotalBooks = c.TotalBooks
                }).ToList()
            })
            .ToList();

        return groupedCategories;
    }

    // Ordered books starting from a specific letter -> by BOOKSHELF & CATEGORY
    public async Task<List<OrderedBookshelfCategoryWithBooksDto>> GetOrderedBooksGroupedByCategories(
        string startFrom, string bookshelfTitle, string categoryTitle, int? maxResults)
    {
        var categoriesQuery = _dbContext.Categories
            .Include(c => c.Bookshelf)
            .Where(c => c.Bookshelf.Title == bookshelfTitle && c.Title == categoryTitle);

        var categories = await categoriesQuery.ToListAsync();

        var categoryIds = categories.Select(c => c.Id).ToList();

        var booksQuery = _dbContext.BooksCategories
            .AsNoTracking()
            .Where(bc => categoryIds.Contains(bc.CategoryId))
            .Include(bc => bc.Book)
                .ThenInclude(b => b.Author);

        var books = await booksQuery.ToListAsync();

        var alphabetRange = GetAlphabetRange(startFrom, 4);

        var groupedCategories = categories
            .GroupBy(c => new { c.Bookshelf.Id, c.Bookshelf.Title })
            .OrderBy(g => g.Key.Id)
            .Select(g => new OrderedBookshelfCategoryWithBooksDto
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                Categories = g.Select(c => new OrderedExploreCategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    TotalBooks = books.Count(bc => bc.CategoryId == c.Id),
                    Books = books
                        .Where(bc => bc.CategoryId == c.Id)
                        .Select(bc => bc.Book)
                        .Where(book => char.IsLetter(GetFirstValidCharacter(book.Title)))
                        .GroupBy(book => GetFirstValidCharacter(book.Title).ToString().ToUpper())
                        .Where(group => alphabetRange.Contains(group.Key[0]))
                        .OrderBy(group => group.Key)
                        .ToDictionary(
                            group => group.Key,
                            group => group
                                .Where(book => book.Title.Contains(group.Key[0], StringComparison.OrdinalIgnoreCase))
                                .OrderBy(book => book.Title)
                                .Take(maxResults ?? int.MaxValue)
                                .ToList()
                        )
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

        var url = "";
        if (!string.IsNullOrEmpty(book.HtmlContentUrl))
        {
            url = $"https://www.gutenberg.org{book.HtmlContentUrl}";
        }
        else if (!string.IsNullOrEmpty(book.HtmlAsSubmittedUrl))
        {
            url = $"https://www.gutenberg.org{book.HtmlAsSubmittedUrl}";
        }
        else if (!string.IsNullOrEmpty(book.HtmlNoImagesUrl))
        {
            url = $"https://www.gutenberg.org{book.HtmlNoImagesUrl}";
        }
        else
        {
            url = $"https://www.gutenberg.org{book.PlainTextUrl}";
        }
        
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        // extract the content between the pg-header and pg-footer sections
        var content = await BookContentUtil.GetContentBetweenSectionsAsync(html);

        // remove any images from the content
        content = new Regex("<img[^>]+>").Replace(content, "");

        bookWithContent.Content = content;

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

    public async Task<UserBookReadingTracker> GetUserReadingTimeSpentAsync(int bookId, string username)
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

    // TODO de mutat in library user
    public async Task<int> GetUserTotalReadingTimeSpentAsync(int userId)
    {
        var user = await _dbContext.Users
            .FindAsync(userId);
        if (user == default) throw new Exception("Invalid user.");
        
        var userReadingBook = await _dbContext.UserReadingBooks
            .Where(c => c.User.Id == userId)
            .SumAsync(b => b.MinutesSpent);

        return userReadingBook;
    }

    // TODO de mutat in library user
    public async Task<int> GetUserTotalReadingTimeSpentByUsernameAsync(string username)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u .Username == username);
        if (user == default) throw new Exception("Invalid user.");
        
        var userReadingBook = await _dbContext.UserReadingBooks
            .Where(c => c.User.Username == username)
            .SumAsync(b => b.MinutesSpent);

        return userReadingBook;
    }

    public async Task<int> UpdateUserReadingTimeSpentAsync(int bookId, string username, int timeSpent)
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
            var newReadingBook = new UserBookReadingTracker
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
        
        return userReadingBook.MinutesSpent;
    }

    public async Task<bool> SetFinishedReadingBookByIdAsync(int bookId, string username, int timeSpent)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        if (book == default) throw new Exception("Invalid book id.");

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Username == username);
        if (user == default) throw new Exception("Invalid user.");
        
        var existingEntity =
            await _dbContext.UserReadingBooks
                .SingleOrDefaultAsync(c => c.User.Username == username & c.BookId == bookId);
        if (existingEntity is { IsBookFinished: true })
        {
            throw new Exception("Already finished reading this book.");
        }

        var totalTimeResponse = await GetReadingTimeOfBookContent(bookId);
        var totalMinutesConverted = TimeConvertor.ConvertResponseMinutes(totalTimeResponse);

        if (timeSpent < totalMinutesConverted) throw new Exception("Time spent reading is lower than average reading time.");

        existingEntity.MinutesSpent = timeSpent;
        existingEntity.IsBookFinished = true;
        existingEntity.Timestamp = DateTime.Now.ToString("dd/MM/yyyy");

        _dbContext.UserReadingBooks.Update(existingEntity);
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

    public async Task<bool> SetOrRemoveFavoriteBookAsync(string username, int bookId)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Username == username);
        if (user == null) throw new UnauthorizedAccessException();
        
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book == null) throw new Exception("Book not found");
        
        var checkExistsEntity = await _dbContext.UserFavoriteBooks.SingleOrDefaultAsync(
            fav => fav.User.Username == username & fav.BookId == bookId
        );

        if (checkExistsEntity == null) 
        {
            // add
            var newFavorite = new UserFavoriteBook
            {
                UserId = user.Id,
                BookId = book.Id
            };

            await _dbContext.UserFavoriteBooks.AddAsync(newFavorite);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // remove
        _dbContext.UserFavoriteBooks.Remove(checkExistsEntity);
        await _dbContext.SaveChangesAsync();
        return false;
    }

    private char GetFirstValidCharacter(string title)
    {
        var regex = new Regex("[^a-zA-Z0-9]");
        var replacedTitle = regex.Replace(title, string.Empty);

        if (string.IsNullOrEmpty(replacedTitle))
        {
            return title[0];
        }

        return replacedTitle[0];
    }

    private static List<char> GetAlphabetRange(string startFrom, int rangeSize)
    {
        var startChar = char.ToUpper(startFrom[0]);
        var endChar = (char)Math.Min(startChar + rangeSize, 'Z');
        return Enumerable.Range(startChar, endChar - startChar + 1).Select(c => (char)c).ToList();
    }
}