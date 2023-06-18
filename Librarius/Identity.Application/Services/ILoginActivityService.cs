namespace Identity.Application.Services;

public interface ILoginActivityService
{
    Task<bool> CreateLoginActivityAsync(int accountId, string date);

    Task CheckForActivityTrophies(string criterion);
}