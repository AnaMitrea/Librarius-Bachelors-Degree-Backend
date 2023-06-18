namespace Library.Application.DTOs.Trophy;

public class CategoryBookRequest
{
    public int CategoryId { get; set; }
    
    public int ReadingBooksCounter { get; set; }
    
    public bool CanCheckWin { get; set; }
}