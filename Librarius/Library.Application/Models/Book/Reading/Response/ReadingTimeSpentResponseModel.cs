namespace Library.Application.Models.Book.Reading.Response;

public class ReadingTimeSpentResponseModel
{
    public int BookId { get; set; }
    
    public int TimeSpent { get; set; }
    
    public bool IsBookFinished { get; set; }
}