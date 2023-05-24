namespace Library.Application.Models.Reviews.Request;

public class ReviewRequestModel
{
    public int BookId { get; set; }
    
    public int MaxResults { get; set; }
    
    public string SortBy { get; set; }
    
    public int StartIndex { get; set; }
}