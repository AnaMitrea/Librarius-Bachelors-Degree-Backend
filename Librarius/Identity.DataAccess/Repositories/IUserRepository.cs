using Identity.DataAccess.DTOs;

namespace Identity.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<IEnumerable<UserLeaderboardByPointsDto>> GetAllUsersByPointsDescAsync();
}