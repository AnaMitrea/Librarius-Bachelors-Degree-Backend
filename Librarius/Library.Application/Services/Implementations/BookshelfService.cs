using AutoMapper;
using Library.Application.Models.Bookshelf.Response;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class BookshelfService : IBookshelfService
{
    private readonly IBookshelfRepository _bookshelfRepository;
    private readonly IMapper _mapper;

    public BookshelfService(IBookshelfRepository bookshelfRepository, IMapper mapper)
    {
        _bookshelfRepository = bookshelfRepository;
        _mapper = mapper;
    }
    
    public async Task<List<BookshelfResponseModel>> GetAllAsync()
    {
        var bookshelves = await _bookshelfRepository.GetAllAsync();

        return _mapper.Map<List<BookshelfResponseModel>>(bookshelves);
    }

    public async Task<List<BookshelfWithCategoriesResponseModel>> GetAllWithCategoryAsync()
    {
        var bookshelves = await _bookshelfRepository.GetAllWithCategoryAsync();

        return _mapper.Map<List<BookshelfWithCategoriesResponseModel>>(bookshelves);
    }
}