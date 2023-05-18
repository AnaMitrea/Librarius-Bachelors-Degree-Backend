namespace Library.Application.Models.Book.Trending;

public class BookTrendingResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Author { get; set; }

    public string CoverImageUrl { get; set; }
}