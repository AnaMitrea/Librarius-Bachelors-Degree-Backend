namespace Library.Application.Models.Book.Author;

public class MaterialsResponseModel
{
    public string Title { get; set; }
    
    public ICollection<BookNoCategoriesResponseModel> Books { get; set; }

    public int Count => Books.Count;
}