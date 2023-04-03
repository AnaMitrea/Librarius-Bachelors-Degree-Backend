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
    
    public async Task<BookResponseModel> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);

        return _mapper.Map<BookResponseModel>(book);
    }
}