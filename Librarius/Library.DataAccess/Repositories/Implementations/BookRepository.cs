using System.Text.RegularExpressions;
using Library.DataAccess.DTOs;
using Library.DataAccess.Entities;
using Library.DataAccess.Persistence;
using Library.DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public partial class BookRepository : IBookRepository
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
            .SingleOrDefaultAsync(book => book.Id == bookId);
        
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
    
    public async Task<Dictionary<string, List<Book>>> GetBooksGroupedByBookshelf()
    {
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
         * select only the Title of the Bookshelf entity. The AsNoTracking() method
         * is called to disable change tracking for the entities returned by the query. 
         */
        var booksGroupedByBookshelf = await _dbContext.BooksCategories
            .AsNoTracking()
            .Select(bc => new { BookshelfTitle = bc.Category.Bookshelf.Title, Book = bc.Book })
            .ToListAsync();
        
        var groupedBooks = booksGroupedByBookshelf
            .GroupBy(bgbb => bgbb.BookshelfTitle)
            .ToDictionary(g => g.Key, g => g.Select(bgbb => bgbb.Book).ToList());
        
        return groupedBooks;
    }

    public async Task<BookWithContent?> GetReadingBookByIdAsync(int bookId)
    {
        var book = await _dbContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
    
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
    
        var bookWithContent = new BookWithContent
        {
            Id = book.Id
        };
    
        if (!string.IsNullOrEmpty(book.HtmlContentUrl))
        {
            // get the HTML content from the book's URL
            var url = $"https://www.gutenberg.org{book.HtmlContentUrl}";
            
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            // extract the content between the pg-header and pg-footer sections
            var content = await BookContentUtil.GetContentBetweenSectionsAsync(html);

            // remove any images from the content
            content = MyRegex().Replace(content, "");

            bookWithContent.Content = content;
        }

        return bookWithContent;
    }


    public async Task<IEnumerable<Book?>> GetTrendingNowBooksAsync()
    {
        // TODO implement
        var result = await _dbContext.Books.Take(10).ToListAsync();

        return result;
    }
    
    public async Task<IEnumerable<Book?>> GetTrendingWeekBooksAsync()
    {
        // TODO implement
        var result = await _dbContext.Books.Take(10).ToListAsync();

        return result;
    }

    [GeneratedRegex("<img[^>]+>")]
    private static partial Regex MyRegex();
}