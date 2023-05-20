using Library.DataAccess.Entities;

namespace Library.DataAccess.DTOs;

public class BookWithContentDto : Entity
{
    public string Content { get; set; }
}