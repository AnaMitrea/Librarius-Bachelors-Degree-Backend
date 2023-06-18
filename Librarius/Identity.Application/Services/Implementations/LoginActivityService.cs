using Identity.DataAccess.Repositories;

namespace Identity.Application.Services.Implementations;

public class LoginActivityService : ILoginActivityService
{
    private readonly ILoginActivityRepository _activityRepository;

    public LoginActivityService(ILoginActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<bool> CreateLoginActivityAsync(int accountId, string date)
    {
        return  await _activityRepository.CreateLoginActivityAsync(accountId, date);
    }

    public async Task CheckForActivityTrophies(string criterion)
    {
        throw new NotImplementedException();
    }
}