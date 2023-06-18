namespace Identity.Application.Services;

public interface ITriggerRewardService
{
    Task TriggerRequestToTrophyChecker(string token);

    Task<bool> TriggerUpdateActivity(string criterion, bool canCheckWin, string token);
}