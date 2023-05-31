using AutoMapper;
using Library.Application.Models.Book.Author;
using Library.Application.Models.SearchBar;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorService(IMapper mapper, IAuthorRepository authorRepository)
    {
        _mapper = mapper;
        _authorRepository = authorRepository;
    }

    public async Task<AuthorResponseModel> GetAuthorInformationByIdAsync(int id)
    {
        var response = await _authorRepository.GetAuthorInformationByIdAsync(id);

        return _mapper.Map<AuthorResponseModel>(response);
    }

    public async Task<ICollection<MaterialsResponseModel>> GetAuthorBooksAsync(MaterialRequestModel requestModel)
    {
        var response = 
            await _authorRepository.GetAuthorBooksAsync(requestModel.AuthorId, requestModel.SortingOption);

        return _mapper.Map<ICollection<MaterialsResponseModel>>(response);
    }

    public async Task<IEnumerable<AuthorMinimalResponseModel>> SearchAuthorByFilterAsync(SearchBarRequestModel requestModel)
    {
        var response = await _authorRepository.SearchBooksByFilterAsync(requestModel.SearchBy, requestModel.MaxResults);

        return _mapper.Map<IEnumerable<AuthorMinimalResponseModel>>(response);
    }
}