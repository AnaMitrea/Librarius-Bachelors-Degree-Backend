namespace Library.Application.Models.LibraryUser.Response;

public class UserResponseModel
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string NameInitial 
        => string.IsNullOrEmpty(Username) 
        ? string.Empty 
        : Username[..1].ToUpper();
}