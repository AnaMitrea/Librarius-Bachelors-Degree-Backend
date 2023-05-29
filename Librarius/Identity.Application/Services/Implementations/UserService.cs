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
}