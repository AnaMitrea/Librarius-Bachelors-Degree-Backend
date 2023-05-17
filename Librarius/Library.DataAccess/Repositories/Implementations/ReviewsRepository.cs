using Library.DataAccess.Entities;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class ReviewsRepository : IReviewsRepository
{
    private readonly DatabaseContext _dbContext;

    public ReviewsRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }
    
    public async Task<ICollection<Review>> GetAllForBookByIdAsync(int id, int maxResults, string sortBy, int startIndex)
    {
        if (sortBy == "MostRecent")
        {
            var reviews = await _dbContext.Reviews
                .Include(r => r.User)
                .Where(r => r.BookId == id)
                .ToListAsync();

            return reviews
                .OrderByDescending(r 
                    => DateTime.ParseExact(r.Timestamp, "dd/MM/yyyy", null))
                .Skip(startIndex)
                .Take(maxResults)
                .ToList();
        }

        return await _dbContext.Reviews
            .Include(r => r.User)
            .Where(r => r.BookId == id)
            .OrderByDescending(r => r.LikesCount)
            .Skip(startIndex)
            .Take(maxResults)
            .ToListAsync();
    }
    
    public async Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false; // Invalid token or user not authenticated
        }

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return false; // User not found
        }

        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return false; // Review not found
        }

        var existingLike = await _dbContext.ReviewLikedBys
            .SingleOrDefaultAsync(l => l.UserId == user.Id && l.ReviewId == reviewId);

        if (isLiked)
        {
            if (existingLike != null)
            {
                return false; // User has already liked this review
            }

            var newLike = new ReviewLikedBy
            {
                UserId = user.Id,
                ReviewId = reviewId
            };

            _dbContext.ReviewLikedBys.Add(newLike);
            review.LikesCount++; // Increment the likes count
        }
        else
        {
            if (existingLike == null)
            {
                return false; // User has not liked this review
            }

            _dbContext.ReviewLikedBys.Remove(existingLike);
            review.LikesCount--; // Decrement the likes count
        }

        await _dbContext.SaveChangesAsync();

        return true;
    }
}