using AutoMapper;
using Identity.Application.Models.Requests;
using Identity.DataAccess.Repositories;

namespace Identity.Application.Services.Implementations;

public class AccountActivityService : IAccountActivityService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;

    public AccountActivityService(IAccountRepository accountRepository, IMapper mapper)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
    }
    
    public async Task<AuthenticationResponseModel?> UpdateUserActivity(string username)
    {
        var today = DateTime.Now.Date;
        var yesterday = today.AddDays(-1);
        
        var todayFormatted = today.Date.ToString("dd/MM/yyyy");
        var yesterdayFormatted = yesterday.Date.ToString("dd/MM/yyyy");
        
        var account = await _accountRepository.GetUserInformationAsync(username);
        if (account == null) return null;
        
        var lastLogin = account.LastLogin;

        if (todayFormatted == lastLogin)
            return _mapper.Map<AuthenticationResponseModel>(account);
        
        int newStreak;
        if (lastLogin == yesterdayFormatted)
        {
            newStreak = ++account.CurrentStreak;
        }
        else
        {
            newStreak = 1;
            account.CurrentStreak = newStreak;
        }
        
        if (account.LongestStreak < newStreak)
        {
            account.LongestStreak = newStreak;
        }

        account.LastLogin = todayFormatted;

        var updatedAccount = await _accountRepository.UpdateUserInformationAsync(account);

        return _mapper.Map<AuthenticationResponseModel>(updatedAccount);
    }
}