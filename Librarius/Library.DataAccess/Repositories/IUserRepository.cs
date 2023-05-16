using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<User> GetUserById(int id);
}