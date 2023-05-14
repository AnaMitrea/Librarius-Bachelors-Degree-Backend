﻿using Library.Application.Models.Category;

namespace Library.Application.Models.Bookshelf;

public class BookshelfWithCategoriesResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public IEnumerable<CategoryResponseModel> Categories { get; set; }
}