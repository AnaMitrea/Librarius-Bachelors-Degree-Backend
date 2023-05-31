using AutoMapper;
using Identity.DataAccess.Repositories;

namespace Identity.Application.Services.Implementations;

public class LoginActivityService : ILoginActivityService
{
    private readonly IMapper _mapper;
    private readonly ILoginActivityRepository _activityRepository;

    public LoginActivityService(ILoginActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateLoginActivityAsync(int accountId, string date)
    {
        return  await _activityRepository.CreateLoginActivityAsync(accountId, date);
    }
}