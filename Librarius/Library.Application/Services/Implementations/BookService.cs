﻿using AutoMapper;
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