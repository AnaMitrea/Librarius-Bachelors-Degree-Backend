﻿using Library.Application.Models.Book.Author;

namespace Library.Application.Services;

public interface IAuthorService
{
    Task<AuthorResponseModel> GetAuthorInformationByIdAsync(int id);

    Task<ICollection<MaterialsResponseModel>> GetAuthorBooksAsync(MaterialRequestModel requestModel);
}