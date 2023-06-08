namespace Library.Application.Services;

public interface ITriggerRewardService
{
    Task TriggerRequestToTrophyChecker();

    Task TriggerUpdateTotalReadingTime(int userId, int minutesReadCounter);
}