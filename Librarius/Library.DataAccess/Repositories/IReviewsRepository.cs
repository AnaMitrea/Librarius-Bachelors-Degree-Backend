using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.BookRelated;

namespace Library.DataAccess.Repositories;

public interface IReviewsRepository
{
    Task<ICollection<Review>> GetAllForBookByIdAsync(int id, int maxResults, string sortBy, int startIndex);

    Task<bool> SetUserReviewByBookIdAsync(UserReviewRequestDto requestDto, string username);
    
    Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked);
}