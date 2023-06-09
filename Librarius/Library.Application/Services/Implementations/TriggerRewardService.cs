using System.Net.Http.Headers;
using System.Net.Http.Json;
using Library.Application.DTOs.Trophy;

namespace Library.Application.Services.Implementations;

public class TriggerRewardService : ITriggerRewardService
{
    private readonly HttpClient _httpClient;
    
    private const string TrophyRewardUrl = "http://localhost:5164/api/trophy/reward/check-win";
    private const string UpdateReadingTimeUrl = "http://localhost:5164/api/trophy/reading-time/reward/update-activity";
    private const string UpdateReadingBookUrl = "http://localhost:5164/api/trophy/reading-books/reward/update-activity";
    
    public TriggerRewardService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task TriggerRequestToTrophyChecker(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(TrophyRewardUrl);

        if (response.IsSuccessStatusCode)
        {
            
        }
    }
    
    public async Task TriggerUpdateTotalReadingTime(int minutesReadCounter, bool canCheckWin, string token)
    {
        var body = new ReadingTimeRequest
        {
            MinutesReadCounter = minutesReadCounter,
            CanCheckWin = canCheckWin
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(UpdateReadingTimeUrl,  body);
        
        response.EnsureSuccessStatusCode();
    }

    public async Task TriggerUpdateTotalReadingBooks(int booksReadCounter, bool canCheckWin, string token)
    {
        var body = new ReadingBooksRequest
        {
            ReadingBooksCounter = booksReadCounter,
            CanCheckWin = canCheckWin
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(UpdateReadingBookUrl,  body);

        response.EnsureSuccessStatusCode();
    }
}