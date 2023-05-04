using AutoMapper;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Identity.DataAccess.DTOs;
using Identity.DataAccess.Repositories;

namespace Identity.Application.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
    }

    public async Task<UserAccountModel> CreateAccountAsync(RegisterRequestModel registerRequest)
    {
        var response = await _accountRepository.CreateAccountAsync(new RegisterUserModel
        {
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            Password = registerRequest.Password
        });

        return _mapper.Map<UserAccountModel>(response);
    }

    public async Task<UserAccountModel?> GetAccountAsync(string username, string password)
    {
        var response = await _accountRepository.GetAccountAsync(username, password);

        return response == null ? null : _mapper.Map<UserAccountModel>(response);
    }
    
    public async Task<DashboardUserModel?> GetUserInformationAsync(string username)
    {
        var response = await _accountRepository.GetUserInformationAsync(username);

        return response == null ? null : _mapper.Map<DashboardUserModel>(response);
    }

    public async Task<bool> CheckUsernameExistence(string username)
    {
       return await _accountRepository.CheckUsernameExistence(username);
    }

    public async Task<bool> CheckEmailExistence(string email)
    {
        return await _accountRepository.CheckEmailExistence(email);
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