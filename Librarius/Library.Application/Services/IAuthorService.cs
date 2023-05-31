using Library.Application.Models.Book.Author;
using Library.Application.Models.SearchBar;

namespace Library.Application.Services;

public interface IAuthorService
{
    Task<AuthorResponseModel> GetAuthorInformationByIdAsync(int id);

    Task<ICollection<MaterialsResponseModel>> GetAuthorBooksAsync(MaterialRequestModel requestModel);
    
    Task<IEnumerable<AuthorMinimalResponseModel>> SearchAuthorByFilterAsync(SearchBarRequestModel requestModelSearchBy);
}