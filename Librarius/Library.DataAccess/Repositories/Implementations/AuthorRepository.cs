using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly DatabaseContext _dbContext;

    public AuthorRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Author> GetAuthorInformationByIdAsync(int id)
    {
        var author = await _dbContext.Authors
            .SingleOrDefaultAsync(a => a.Id == id);

        if (author == default) throw new Exception("The author id is invalid.");

        return author;
    }

    public async Task<ICollection<AuthorMaterialsDto>> GetAuthorBooksAsync(int id, int sortingOption)
    {
        var author = await GetAuthorInformationByIdAsync(id);

        var authorBooks = await _dbContext.Books
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .Where(b => b.AuthorId == author.Id)
            .ToListAsync();

        var groupedBooksByCategory = authorBooks
            .SelectMany(b => b.BookCategories.Select(bc => new { Book = b, bc.Category }))
            .GroupBy(x => x.Category);

        var authorMaterials = new List<AuthorMaterialsDto>();

        foreach (var group in groupedBooksByCategory)
        {
            var categoryTitle = group.Key.Title;
            var booksInCategory = group.Select(x => x.Book).ToList();

            authorMaterials.Add(new AuthorMaterialsDto
            {
                CategoryTitle = categoryTitle,
                Books = booksInCategory
            });
        }

        authorMaterials = sortingOption switch
        {
            1 =>
                // Alphabetically by book title
                authorMaterials.OrderBy(a => a.CategoryTitle).ThenBy(a => a.Books.OrderBy(b => b.Title)).ToList(),
            2 =>
                // By ascending reviews count
                authorMaterials.OrderBy(a => a.Books.Sum(b => b.Reviews?.Count ?? 0)).ToList(),
            3 =>
                // By descending reviews count
                authorMaterials.OrderByDescending(a => a.Books.Sum(b => b.Reviews?.Count ?? 0))
                    .ToList(),
            _ => throw new Exception("Invalid sorting option.")
        };

        return authorMaterials;
    }

    public async Task<IEnumerable<Author>> SearchBooksByFilterAsync(string searchByKey, int maxResults)
    {
        var filteredAuthors = await _dbContext.Authors
            .Where(author => author.Name.ToUpper().Contains(searchByKey.ToUpper()))
            .Take(maxResults)
            .ToListAsync();

        return filteredAuthors;
    }
}