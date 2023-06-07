using Microsoft.EntityFrameworkCore;
using Trophy.DataAccess.Entities;
using Trophy.DataAccess.Persistence;

namespace Trophy.DataAccess.Repositories.Implementations;

public class LevelAssignRepository : ILevelAssignRepository
{
    private readonly DatabaseContext _dbContext;

    public LevelAssignRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetLevelByPointsAsync(int points)
    {
        var level = await _dbContext.Levels
            .FirstOrDefaultAsync(l => l.MinPoints <= points && l.MaxPoints >= points);

        if (level == null) throw new Exception("Level cannot be obtained.");
        
        return level.Name;
    }

    public async Task<string> GetNextLevelByPointsAsync(int points)
    {
        var currentLevel = await _dbContext.Levels
            .FirstOrDefaultAsync(l => l.MinPoints <= points && l.MaxPoints >= points);

        if (currentLevel == null)
        {
            return "Invalid Next Level";
        }

        var nextLevel = await _dbContext.Levels
            .OrderBy(l => l.MinPoints)
            .FirstOrDefaultAsync(l => l.MinPoints > currentLevel.MinPoints);

        return nextLevel != null ? nextLevel.Name : currentLevel.Name;
    }

    public async Task<IEnumerable<Level>> GetLevels()
    {
        return await _dbContext.Levels.ToListAsync();
    }
}