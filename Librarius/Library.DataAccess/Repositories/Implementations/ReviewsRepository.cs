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
            .OrderByDescending(r => r.Likes)
            .Skip(startIndex)
            .Take(maxResults)
            .ToListAsync();
    }
}