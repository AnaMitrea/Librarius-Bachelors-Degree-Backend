using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories;

public interface ITrophyRepository
{
    Task<IEnumerable<Trophy>> GetTrophiesByCategoryAsync(string category, bool canTakeMax16 = false);
}