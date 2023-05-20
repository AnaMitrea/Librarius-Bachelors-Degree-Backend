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
}