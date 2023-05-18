namespace Library.DataAccess.Entities.Library;

public class Bookshelf : Entity
{
    public string Title { get; set; }

    public IEnumerable<Category> Categories { get; set; }
}

