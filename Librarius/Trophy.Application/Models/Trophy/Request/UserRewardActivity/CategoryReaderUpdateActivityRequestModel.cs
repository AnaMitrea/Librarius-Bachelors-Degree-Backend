namespace Trophy.Application.Models.Trophy.Request.UserRewardActivity;

public class CategoryReaderUpdateActivityRequestModel
{
    public string CategoryId { get; set; }
    
    public int ReadingBooksCounter { get; set; }
    
    public bool CanCheckWin { get; set; }
}