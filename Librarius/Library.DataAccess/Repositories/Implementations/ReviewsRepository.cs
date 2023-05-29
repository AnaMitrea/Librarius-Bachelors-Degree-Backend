using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.User;
using Library.DataAccess.Entities.BookRelated;
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

    public async Task<bool> SetUserReviewByBookIdAsync(UserReviewRequestDto requestDto, string username)
    {
        if (requestDto.ReviewContent.Length is < 50 or > 2000)
            throw new Exception("Review length is not between 50 and 2000 characters.");
        
        if (requestDto.Rating is < 1 or > 10)
            throw new Exception("Rating must be between 1 and 10.");
        
        if (string.IsNullOrEmpty(username)) throw new Exception("Authorization failed.");
        
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new Exception("User not found.");

        var book = await _dbContext.Books.FindAsync(requestDto.BookId);
        if (book == null) throw new Exception("Book not found.");

        var newReview = new Review
        {
            BookId = book.Id,
            Book = book,
            Content = requestDto.ReviewContent,
            Timestamp = DateTime.Now.ToString("dd/MM/yyyy"),
            Rating = requestDto.Rating,
            LikesCount = 0,
            User = user,
            UserId = user.Id,
        };
        
        _dbContext.Reviews.Add(newReview);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteReviewByIdAsync(int id)
    {
        var review = await _dbContext.Reviews
            .Include(r => r.Likes)
            .SingleOrDefaultAsync(r => r.Id == id);

        if (review == null) return false;

        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateLikeStatusAsync(string username, int reviewId, bool isLiked)
    {
        if (string.IsNullOrEmpty(username)) throw new Exception("Authorization failed.");
        
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user == null) throw new Exception("User not found.");

        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review == null) throw new Exception("Review not found.");

        var existingLike = await _dbContext.ReviewLikedBys
            .SingleOrDefaultAsync(l => l.UserId == user.Id && l.ReviewId == reviewId);

        if (isLiked)
        {
            if (existingLike != null) return false; // User has already liked this review
            
            var newLike = new ReviewLikedBy
            {
                UserId = user.Id,
                ReviewId = reviewId
            };

            _dbContext.ReviewLikedBys.Add(newLike);
            review.LikesCount++;
        }
        else
        {
            if (existingLike == null) return false; // User has not liked this review
            
            _dbContext.ReviewLikedBys.Remove(existingLike);
            review.LikesCount--;
        }

        await _dbContext.SaveChangesAsync();

        return true;
    }
}