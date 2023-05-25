using Library.Application.Models.LibraryUser;

namespace Library.Application.Models.Reviews;

public class ReviewModel
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public int LikesCount { get; set; }

    public bool Liked { get; set; }
    
    public int Rating { get; set; }
    public string TimeUnit { get; set; }
    public string TimeValue { get; set; }

    public bool IsMyReview { get; set; }
    
    public UserResponseModel User { get; set; }
}