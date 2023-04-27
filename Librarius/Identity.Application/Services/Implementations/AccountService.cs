using AutoMapper;
using Identity.Application.Models.User;
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
    
    public async Task<UserAccountModel?> GetAccountAsync(string username, string password)
    {
        var response = await _accountRepository.GetAccountAsync(username, password);

        return response == null ? null : _mapper.Map<UserAccountModel>(response);
    }
    
    public async Task<UserModel?> GetUserInformationAsync(string username)
    {
        var response = await _accountRepository.GetUserInformationAsync(username);

        return response == null ? null : _mapper.Map<UserModel>(response);
    }
}