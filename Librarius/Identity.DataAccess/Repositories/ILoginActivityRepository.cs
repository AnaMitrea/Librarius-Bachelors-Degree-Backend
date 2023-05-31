namespace Identity.DataAccess.Repositories;

public interface ILoginActivityRepository
{
    Task<bool> CreateLoginActivityAsync(int accountId, string date);
}