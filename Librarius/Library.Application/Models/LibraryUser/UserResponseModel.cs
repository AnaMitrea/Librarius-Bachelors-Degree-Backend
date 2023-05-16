namespace Library.Application.Models.LibraryUser;

public class UserResponseModel
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string NameInitial 
        => string.IsNullOrEmpty(Username) 
        ? string.Empty 
        : Username[..1];
}