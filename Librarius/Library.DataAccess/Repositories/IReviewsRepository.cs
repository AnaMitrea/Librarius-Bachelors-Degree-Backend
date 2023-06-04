using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities.BookRelated;

namespace Library.DataAccess.Repositories;

public interface IReviewsRepository
{
    Task<ICollection<Review>> GetAllForBookByIdAsync(string username, int id, int maxResults, string sortBy, int startIndex);

    Task<bool> SetUserReviewByBookIdAsync(UserReviewRequestDto requestDto, string username);
    
    Task<bool> DeleteReviewByIdAsync(int id);
    
    Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked);
}