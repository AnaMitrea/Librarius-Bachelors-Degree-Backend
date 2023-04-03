﻿using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IBookRepository
{
    Task<Book> GetBookByIdAsync(int id);
    
    // TODO add CRUD operations
}