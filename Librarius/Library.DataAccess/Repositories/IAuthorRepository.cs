using Library.DataAccess.Entities.BookRelated;

namespace Library.DataAccess.Repositories;

public interface IAuthorRepository
{
    Task<Author> GetAuthorInformationByIdAsync(int id);
}