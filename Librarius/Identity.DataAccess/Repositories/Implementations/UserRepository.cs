using System.Globalization;
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

    public async Task<IEnumerable<string>> GetUserDashboardActivityAsync(string username)
    {
        var user = await _dbContext.Accounts.SingleOrDefaultAsync(ac => ac.Username == username);
        if (user == null) throw new Exception("User not found.");

        var activities = await _dbContext.LoginActivities
            .Where(ac => ac.AccountId == user.Id)
            .Select(ac 
                => DateTime.ParseExact(ac.DateTimestamp, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                .ToString("yyyy-MM-dd"))
            .ToListAsync();

        return activities;
    }
}