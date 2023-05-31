using Library.Application.Models.Book;

namespace Library.Application.Models.LibraryUser.Response;

public class UserReadingFeed : UserResponseModel
{
    public BookNoCategoriesResponseModel Book { get; set; }
}