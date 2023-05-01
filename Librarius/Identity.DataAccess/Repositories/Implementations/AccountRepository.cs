﻿using Identity.DataAccess.DTOs;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations;

public class AccountRepository : IAccountRepository
{
    private readonly DatabaseContext _databaseContext;

    public AccountRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
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
            .SingleOrDefaultAsync(user => user.Username == username && user.Password == password);
        
        if (account == default)
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