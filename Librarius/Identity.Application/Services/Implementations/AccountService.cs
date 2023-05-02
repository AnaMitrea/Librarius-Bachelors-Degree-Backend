using AutoMapper;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Identity.DataAccess.DTOs;
using Identity.DataAccess.Repositories;
using UserModel = Identity.Application.Models.User.UserModel;

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
    
    public async Task<UserModel?> GetUserInformationAsync(string username)
    {
        var response = await _accountRepository.GetUserInformationAsync(username);

        return response == null ? null : _mapper.Map<UserModel>(response);
    }

    public async Task<bool> CheckUsernameExistence(string username)
    {
       return await _accountRepository.CheckUsernameExistence(username);
    }

    public async Task<bool> CheckEmailExistence(string email)
    {
        return await _accountRepository.CheckEmailExistence(email);
    }
}