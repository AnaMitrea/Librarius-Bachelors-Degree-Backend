using Identity.DataAccess.DTOs;
using Identity.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<UserLeaderboardByPointsDto>> GetAllUsersByPointsDescAsync()
    {
        var userLeaderboard = await _dbContext.Accounts
            .OrderByDescending(u => u.Points)
            .ToListAsync();

        return userLeaderboard.Select(
            (user, i) => new UserLeaderboardByPointsDto
            {
                Id = user.Id,
                Username = user.Username,
                Position = i + 1,
                Points = user.Points
            }
        ).ToList();
    }

}