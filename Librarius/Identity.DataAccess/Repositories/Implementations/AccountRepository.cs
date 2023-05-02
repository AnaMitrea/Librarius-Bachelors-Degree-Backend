using Identity.DataAccess.DTOs;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Exceptions;
using Identity.DataAccess.Persistence;
using Identity.DataAccess.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.DataAccess.Repositories.Implementations;

public class AccountRepository : IAccountRepository
{
    private readonly DatabaseContext _databaseContext;
    private readonly string _pepper;

    public AccountRepository(DatabaseContext databaseContext, IConfiguration configuration)
    {
        _databaseContext = databaseContext;
        _pepper = configuration["PepperHash"]!;
    }

    public async Task<Account> CreateAccountAsync(RegisterUserModel registerUser)
    {
        var hashedPassword = PasswordHasher.HashPassword(registerUser.Password, _pepper);

        var account = new Account
        {
            Username = registerUser.Username,
            Password = hashedPassword,
            Email = registerUser.Email,
            CurrentStreak = 1,
            LongestStreak = 1,
            LastLogin = DateTime.Now.Date.ToString("dd/MM/yyyy"),
            Level = "Beginner",
            Points = 0,
            Role = "User"
        };
        var addedAccount = (await _databaseContext.Accounts.AddAsync(account)).Entity;
        await _databaseContext.SaveChangesAsync();
        
        return addedAccount;
    }

    public async Task<bool> CheckUsernameExistence(string username)
    {
        var account = await _databaseContext.Accounts.SingleOrDefaultAsync(user => user.Username == username);
        if (account != default)
        {
            throw new UsernameAlreadyExistsException();
        }

        return false;
    }

    public async Task<bool> CheckEmailExistence(string email)
    {
        var account = await _databaseContext.Accounts.SingleOrDefaultAsync(user => user.Email == email);
        if (account != default)
        {
            throw new EmailAlreadyExistsException();
        }

        return false;
    }

    public async Task<bool> FindAccountByUsernameAsync(string username)
    {
        var account = await _databaseContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == username);
        
        return account != default;
    }

    public async Task<Account?> GetAccountAsync(string username, string password)
    {
        var account = await _databaseContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == username);
        if (account == default)
        {
            throw new Exception("Invalid Credentials.");
        }
        
        var isPasswordValid = PasswordHasher.VerifyPassword(password, account.Password, _pepper);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid Credentials.");
        }
        
        return account;
    }
    
    public async Task<Account?> GetUserInformationAsync(string username)
    {
        var account = await _databaseContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == username);
        
        if (account == default)
        {
            throw new Exception("No account found.");
        }
        
        return account;
    }

    public async Task<Account?> UpdateUserInformationAsync(Account userModel)
    {
        var account = await _databaseContext.Accounts
            .SingleOrDefaultAsync(user => user.Username == userModel.Username);
        
        if (account == default)
        {
            throw new Exception("No account found.");
        }

        account.LastLogin = userModel.LastLogin;
        account.CurrentStreak = userModel.CurrentStreak;
        account.LongestStreak = userModel.LongestStreak;
        await _databaseContext.SaveChangesAsync();
        
        return account;
    }
}