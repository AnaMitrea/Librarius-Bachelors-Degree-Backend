namespace Library.DataAccess.Entities;

public class Category : Entity
{
    public string Title { get; set; }
    
    public string Link { get; set; }

    // many-to-many: books - categories
    public IEnumerable<BookCategory> BookCategories { get; set; }
    
    // many-to-one: categories - 1 bookshelf
    public int BookshelfId { get; set; }
    public Bookshelf Bookshelf { get; set; }
}