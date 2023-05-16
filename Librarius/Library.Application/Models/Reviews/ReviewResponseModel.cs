using Library.Application.Models.LibraryUser;

namespace Library.Application.Models.Reviews;

public class ReviewResponseModel
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public int Likes { get; set; }
    
    public string Timestamp { get; set; }

    public UserResponseModel User { get; set; }
}