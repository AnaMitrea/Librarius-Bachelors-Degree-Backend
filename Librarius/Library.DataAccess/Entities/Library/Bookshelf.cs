namespace Library.DataAccess.Entities.Library;

public class Bookshelf : Entity
{
    public string Title { get; set; }

    // * categories to 1 bookshelf
    public IEnumerable<Category> Categories { get; set; }
}

