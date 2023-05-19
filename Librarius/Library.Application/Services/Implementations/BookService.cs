using AutoMapper;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Trending;
using Library.DataAccess.DTOs;
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
    
    public async Task<IEnumerable<ExploreBookResponseModel>> GetBooksForAllBookshelves()
    {
        var response = await _bookRepository.GetBooksForAllBookshelves();

        return _mapper.Map<IEnumerable<ExploreBookResponseModel>>(response);
    }
    
    public async Task<Dictionary<string, List<BookResponseModel>>> GetBooksGroupedByBookshelf()
    {
        var response = await _bookRepository.GetBooksGroupedByBookshelf();

        return _mapper.Map<Dictionary<string, List<BookResponseModel>>>(response);
    }

    public async Task<BookReadingResponseModel> GetReadingBookByIdAsync(int id)
    {
        var response = await _bookRepository.GetReadingBookByIdAsync(id);

        return _mapper.Map<BookReadingResponseModel>(response);
    }
    
    public async Task<int> GetBookContentWordCount(int id)
    {
        return await _bookRepository.CountWordsInResponseAsync(id);
    }
    
    public async Task<ReadingTimeResponse> GetReadingTimeOfBookContent(int id)
    {
        return await _bookRepository.GetReadingTimeOfBookContent(id);
    }

    public async Task<bool> SetFinishedReadingBookByIdAsync(CompletedBookRequestModel requestModel, string username)
    {
        var response = await _bookRepository.SetFinishedReadingBookByIdAsync(
            requestModel.Id,
            username,
            requestModel.TimeSpent
        );

        return response;
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