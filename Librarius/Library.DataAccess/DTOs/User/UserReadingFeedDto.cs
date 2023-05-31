using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs.User;

public class UserReadingFeedDto
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string NameInitial 
        => string.IsNullOrEmpty(Username) 
            ? string.Empty 
            : Username[..1].ToUpper();
    
    public Book Book { get; set; }
}