using AutoMapper;
using Identity.Application.Models.User;
using Identity.DataAccess.Repositories;

namespace Identity.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<UserLeaderboardByPoints>> GetAllUsersByPointsDescAsync()
    {
        var response = await _userRepository.GetAllUsersByPointsDescAsync();
        
        return _mapper.Map<IEnumerable<UserLeaderboardByPoints>>(response);
    }

    public async Task<IEnumerable<string>> GetUserDashboardActivityAsync(string username)
    {
        var response = await _userRepository.GetUserDashboardActivityAsync(username);
        
        return _mapper.Map<IEnumerable<string>>(response);
    }

    public async Task<int> FindUserIdByUsernameAsync(string username)
    {
        return await _userRepository.FindUserIdByUsernameAsync(username);
    }

    public async Task<int> AddPointsToUserAsync(string username, int points)
    {
        return await _userRepository.AddPointsToUserAsync(username, points);;
    }
    
    public async Task<string> SetUserLevelAsync(string username, string level)
    {
        return await _userRepository.SetUserLevelAsync(username, level);
    }
}