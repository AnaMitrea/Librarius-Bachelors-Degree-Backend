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
    
    public async Task<ICollection<Review>> GetAllForBookByIdAsync(int id)
    {
        // Having BookID, i need all the reviews of the certain book including the user information into the response.
        
        var reviews = await _dbContext.Reviews
            .Include(r => r.User)
            .Where(r => r.BookId == id)
            .ToListAsync();

        return reviews;
    }
}