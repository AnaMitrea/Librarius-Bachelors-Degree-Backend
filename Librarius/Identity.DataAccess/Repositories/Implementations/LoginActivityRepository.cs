using Identity.DataAccess.Entities;
using Identity.DataAccess.Persistence;

namespace Identity.DataAccess.Repositories.Implementations;

public class LoginActivityRepository : ILoginActivityRepository
{
    private readonly DatabaseContext _dbContext;

    public LoginActivityRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateLoginActivityAsync(int accountId, string date)
    {
        var activity = new LoginActivity
        {
            AccountId = accountId,
            DateTimestamp = date
        };

        await _dbContext.AddAsync(activity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}