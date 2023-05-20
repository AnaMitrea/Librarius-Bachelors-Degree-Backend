using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs;

public class AuthorMaterialsDto
{
    public string CategoryTitle { get; set; }
    
    public ICollection<Book> Books { get; set; }
}