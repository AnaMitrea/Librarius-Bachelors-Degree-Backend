using AutoMapper;
using Library.Application.Models.Book;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BookService(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }
    
    public async Task<BookResponseModel> GetBookByIdAsync(int id)
    {
        var response = await _bookRepository.GetBookByIdAsync(id);

        return _mapper.Map<BookResponseModel>(response);
    }
    
    public async Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id)
    {
        var response = await _bookRepository.GetBookWithCategoryByIdAsync(id);

        return _mapper.Map<BookResponseModel>(response);
    }

    public async Task<IEnumerable<BookTrendingResponseModel>> GetTrendingNowBooksAsync()
    {
        var response = await _bookRepository.GetTrendingNowBooksAsync();

        return _mapper.Map<IEnumerable<BookTrendingResponseModel>>(response);
    }
    
    public async Task<IEnumerable<BookTrendingResponseModel>> GetTrendingWeekBooksAsync()
    {
        var response = await _bookRepository.GetTrendingWeekBooksAsync();

        return _mapper.Map<IEnumerable<BookTrendingResponseModel>>(response);
    }
}