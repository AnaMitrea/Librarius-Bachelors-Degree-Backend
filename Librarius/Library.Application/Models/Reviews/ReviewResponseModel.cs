using Library.Application.Models.LibraryUser;

namespace Library.Application.Models.Reviews;

public class ReviewResponseModel
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public int Likes { get; set; }
    public string TimeUnit { get; set; }
    
    public string TimeValue { get; set; }

    public UserResponseModel User { get; set; }
}