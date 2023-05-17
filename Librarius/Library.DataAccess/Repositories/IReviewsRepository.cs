using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IReviewsRepository
{
    Task<ICollection<Review>> GetAllForBookByIdAsync(int id, int maxResults, string sortBy, int startIndex);

    Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked);
}