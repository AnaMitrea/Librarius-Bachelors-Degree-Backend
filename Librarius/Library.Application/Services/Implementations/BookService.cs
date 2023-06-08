using AutoMapper;
using Library.Application.Models.Book;
using Library.Application.Models.Book.Explore.Bookshelf;
using Library.Application.Models.Book.Explore.Category;
using Library.Application.Models.Book.Favorite;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Reading.Response;
using Library.Application.Models.Book.Trending;
using Library.Application.Models.SearchBar;
using Library.DataAccess.DTOs;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ITriggerRewardService _triggerRewardService;
    private readonly IMapper _mapper;

    public BookService(IBookRepository bookRepository, IMapper mapper, ITriggerRewardService triggerRewardService)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _triggerRewardService = triggerRewardService;
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
    
    public async Task<Dictionary<string, BooksForBookshelfResponseModel>> GetBooksGroupedByBookshelf(int? maxResults, string? title)
    {
        var response = await _bookRepository.GetBooksGroupedByBookshelf(maxResults, title);

        return _mapper.Map<Dictionary<string, BooksForBookshelfResponseModel>>(response);
    }
    
    public async Task<Dictionary<string, BooksForBookshelfResponseModel>> GetGroupedBookshelves(string? title)
    {
        var response = await _bookRepository.GetGroupedBookshelves(title);

        return _mapper.Map<Dictionary<string, BooksForBookshelfResponseModel>>(response);
    }

    public async Task<Dictionary<string, OrderedBooksForBookshelfResponseModel>> GetOrderedBooksGroupedByBookshelf(int? maxResults,
        string? title)
    {
        var response = await _bookRepository.GetOrderedBooksGroupedByBookshelf(maxResults, title);

        return _mapper.Map<Dictionary<string, OrderedBooksForBookshelfResponseModel>>(response);
    }

    public async Task<List<BooksForCategoryResponseModel>> GetBooksGroupedByCategoryAndBookshelf(int? maxResults, string? title)
    {
        var response = await _bookRepository.GetBooksGroupedByCategoryAndBookshelf(maxResults, title);

        return _mapper.Map<List<BooksForCategoryResponseModel>>(response);
    }

    public async Task<List<OrderedBookshelfCategoryBooksResponseModel>> GetOrderedBooksGroupedByCategories(
        string startFrom, string bookshelfTitle, string categoryTitle, int? maxResults)
    {
        var response = await _bookRepository.GetOrderedBooksGroupedByCategories(
            startFrom, bookshelfTitle, categoryTitle, maxResults);

        return _mapper.Map<List<OrderedBookshelfCategoryBooksResponseModel>>(response);
    }

    public async Task<List<BooksForCategoryResponseModel>> GetGroupedCategoryAndBookshelf(string? title)
    {
        var response = await _bookRepository.GetGroupedCategoryAndBookshelf(title);

        return _mapper.Map<List<BooksForCategoryResponseModel>>(response);
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
    
    public async Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int id)
    {
        return await _bookRepository.GetReadingTimeOfBookContent(id);
    }
    
    public async Task<bool> CheckIsBookFinishedReading(int bookId, string username)
    {
        return await _bookRepository.CheckIsBookFinishedReading(bookId, username);
    }

    public async Task<ReadingTimeSpentResponseModel> GetUserReadingTimeSpentAsync(ReadingRequestModel requestModel, string username)
    {
        var response = await _bookRepository.GetUserReadingTimeSpentAsync(
            requestModel.BookId,
            username
        );

        return _mapper.Map<ReadingTimeSpentResponseModel>(response);
    }

    public async Task<int> GetUserTotalReadingTimeSpentByIdAsync(int userId)
    {
        var response = await _bookRepository.GetUserTotalReadingTimeSpentAsync(userId);

        return response;
    }
    
    public async Task<int> GetUserTotalReadingTimeSpentByUsernameAsync(string username)
    {
        var response = await _bookRepository.GetUserTotalReadingTimeSpentByUsernameAsync(username);

        return response;
    }

    public async Task<int> UpdateUserReadingTimeSpentAsync(UserReadingBookRequestModel requestModel, string username)
    {
        var response = await _bookRepository.UpdateUserReadingTimeSpentAsync(
            requestModel.BookId,
            username,
            requestModel.TimeSpent
        );

        return response;
    }

    public async Task<bool> SetFinishedReadingBookByIdAsync(UserReadingBookRequestModel requestModel, string username)
    {
        var response = await _bookRepository.SetFinishedReadingBookByIdAsync(
            requestModel.BookId,
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

    public async Task<IEnumerable<BookMinimalResponseModel>> SearchBooksByFilterAsync(SearchBarRequestModel requestModel)
    {
        var response = await _bookRepository.SearchBooksByFilterAsync(requestModel.SearchBy, requestModel.MaxResults);

        return _mapper.Map<IEnumerable<BookMinimalResponseModel>>(response);
    }

    public async Task<bool> SetOrRemoveFavoriteBookAsync(BookFavoriteRequestModel requestModel, string username)
    {
        return await _bookRepository.SetOrRemoveFavoriteBookAsync(username, requestModel.BookId);
    }
}