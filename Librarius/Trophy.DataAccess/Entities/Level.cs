namespace Trophy.DataAccess.Entities;

public class Level : Entity
{
    public string Name { get; set; }
    
    public int MinPoints { get; set; }
    
    public int MaxPoints { get; set; }
}