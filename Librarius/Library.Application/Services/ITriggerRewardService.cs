namespace Library.Application.Services;

public interface ITriggerRewardService
{
    Task TriggerRequestToTrophyChecker(string token);

    Task TriggerUpdateTotalReadingTime(int minutesReadCounter, bool canCheckWin, string token);
    
    Task TriggerUpdateTotalReadingBooks(int booksReadCounter, bool canCheckWin, string token);
}