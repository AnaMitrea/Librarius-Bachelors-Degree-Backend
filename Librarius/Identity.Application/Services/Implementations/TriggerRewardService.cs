using System.Net.Http.Headers;
using System.Net.Http.Json;
using Identity.Application.Models.TrophyTrigger;
using Identity.Application.Utilities;

namespace Identity.Application.Services.Implementations;

public class TriggerRewardService : ITriggerRewardService
{
    private readonly HttpClient _httpClient;
    
    private const string TrophyRewardUrl = "http://localhost:5164/api/trophy/reward/check-win";
    private const string UpdateActivityUrl = "http://localhost:5164/api/trophy/activities/reward/update-activity";
    
    public TriggerRewardService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task TriggerRequestToTrophyChecker(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(TrophyRewardUrl);
    }

    public async Task<bool> TriggerUpdateActivity(string criterion, bool canCheckWin, string token)
    {
        var body = new ActivityTrigger
        {
            Criterion = criterion,
            CanCheckWin = canCheckWin
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(UpdateActivityUrl,  body);
        
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return Utils.GetJsonPropertyAsBool(jsonResponse, new[] { "result" });
    }
}