using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.BookRelated;

namespace Library.DataAccess.Repositories;

public interface IAuthorRepository
{
    Task<Author> GetAuthorInformationByIdAsync(int id);

    Task<ICollection<AuthorMaterialsDto>> GetAuthorBooksAsync(int id, int sortingOption);
}