using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IReviewsRepository
{
    Task<ICollection<Review>> GetAllForBookByIdAsync(int id);
}