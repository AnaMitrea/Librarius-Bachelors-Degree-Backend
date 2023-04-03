namespace Library.DataAccess.Entities;

public class Bookshelf : Entity
{
    public string Title { get; set; }

    public IEnumerable<Category> Categories { get; set; }
}

