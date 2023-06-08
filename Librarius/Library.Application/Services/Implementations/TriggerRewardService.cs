using System.Net.Http.Headers;
using System.Net.Http.Json;
using Library.Application.DTOs.Trophy;

namespace Library.Application.Services.Implementations;

public class TriggerRewardService : ITriggerRewardService
{
    private readonly HttpClient _httpClient;
    
    private const string TrophyRewardUrl = "http://localhost:5164/api/trophy/reward/check-win";
    private const string UpdateReadingTimeUrl = "http://localhost:5164/api/trophy/reading-time/reward/update-activity";
    
    public TriggerRewardService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task TriggerRequestToTrophyChecker()
    {
        var response = await _httpClient.GetAsync(TrophyRewardUrl);

        if (response.IsSuccessStatusCode)
        {
            
        }
    }
    
    public async Task TriggerUpdateTotalReadingTime(int userId, int minutesReadCounter)
    {
        var body = new ReadingTimeRequest
        {
            UserId = userId,
            MinutesReadCounter = minutesReadCounter
        };
        
        var response = await _httpClient.PutAsJsonAsync(UpdateReadingTimeUrl, body);

        if (response.IsSuccessStatusCode)
        {
            
        }
    }
}