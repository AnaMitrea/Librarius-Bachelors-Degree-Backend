using Library.DataAccess.Entities;

namespace Library.DataAccess.DTOs;

public class BookWithContent : Entity
{
    public string Content { get; set; }
}