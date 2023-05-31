namespace Library.Application.Models.Book.Author;

public class AuthorMinimalResponseModel
{
    public string Type => "Author";
    
    public int Id { get; set; }
    
    public string Name { get; set; }
}