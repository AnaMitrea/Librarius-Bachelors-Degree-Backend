namespace Library.Application.Services;

public interface ITriggerRewardService
{
    Task TriggerRequestToTrophyChecker(string token);

    Task<bool> TriggerRewardForLengthyReview(string token);

    Task<bool> TriggerUpdateTotalReadingTime(int minutesReadCounter, bool canCheckWin, string token);
    
    Task<bool> TriggerUpdateTotalReadingBooks(int booksReadCounter, bool canCheckWin, string token);
    
    Task<bool> TriggerUpdateCategoryReadingBook(int counter, int categoryId, bool canCheckWin, string token);
}