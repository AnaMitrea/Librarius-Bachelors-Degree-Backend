using Library.DataAccess.Entities;
using Library.DataAccess.Persistence;

namespace Library.DataAccess.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }
    
    public Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }
}