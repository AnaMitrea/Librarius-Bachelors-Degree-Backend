using System.Text.RegularExpressions;
using Library.DataAccess.DTOs;
using Library.DataAccess.Entities;
using Library.DataAccess.Persistence;
using Library.DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public partial class BookRepository : IBookRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public BookRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
        var book = await _databaseContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
        
        return book;
    }

    public async Task<Book?> GetBookWithCategoryByIdAsync(int bookId)
    {
        var book = await _databaseContext.Books
            .SingleOrDefaultAsync(book => book.Id == bookId);
        
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
        
        // Split query in order to increase performance from 500ms to 100ms !!!
        await _databaseContext.Entry(book)
            .Collection(x => x.BookCategories)
            .Query()
            .Include(x => x.Category)
            .LoadAsync();
    
        return book;
    }

    public async Task<BookWithContent?> GetReadingBookByIdAsync(int bookId)
    {
        var book = await _databaseContext.Books
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
            // var url = "https://www.gutenberg.org" + book.HtmlContentUrl;
            const string url = "https://www.gutenberg.org/cache/epub/1513/pg1513-images.html";
            
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
        var result = await _databaseContext.Books.Take(10).ToListAsync();

        return result;
    }
    
    public async Task<IEnumerable<Book?>> GetTrendingWeekBooksAsync()
    {
        // TODO implement
        var result = await _databaseContext.Books.Take(10).ToListAsync();

        return result;
    }

    [GeneratedRegex("<img[^>]+>")]
    private static partial Regex MyRegex();

    // TODO implement CRUD operations defined in interface
}