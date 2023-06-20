using System.Net.Http.Headers;
using System.Net.Http.Json;
using Library.Application.DTOs.Trophy;
using Library.Application.Utilities;

namespace Library.Application.Services.Implementations;

public class TriggerRewardService : ITriggerRewardService
{
    private readonly HttpClient _httpClient;

    public TriggerRewardService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task TriggerRequestToTrophyChecker(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(Utils.TrophyRewardUrl);
    }

    public async Task<bool> TriggerRewardForLengthyReview(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(Utils.TrophyLengthyReviewRewardUrl);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return Utils.GetJsonPropertyAsBool(responseJson, new[] { "result" });
    }

    public async  Task<bool> TriggerUpdateTotalReadingTime(int minutesReadCounter, bool canCheckWin, string token)
    {
        var body = new ReadingTimeRequest
        {
            MinutesReadCounter = minutesReadCounter,
            CanCheckWin = canCheckWin
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(Utils.UpdateReadingTimeUrl,  body);
        
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return Utils.GetJsonPropertyAsBool(jsonResponse, new[] { "result" });
    }

    public async Task<bool> TriggerUpdateTotalReadingBooks(int booksReadCounter, bool canCheckWin, string token)
    {
        var body = new ReadingBooksRequest
        {
            ReadingBooksCounter = booksReadCounter,
            CanCheckWin = canCheckWin
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(Utils.UpdateReadingBookUrl,  body);

        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return Utils.GetJsonPropertyAsBool(jsonResponse, new[] { "result" });
    }

    public async Task<bool> TriggerUpdateCategoryReadingBook(int counter, int categoryId, bool canCheckWin, string token)
    {
        var body = new CategoryBookRequest
        {
            ReadingBooksCounter = counter,
            CategoryId = categoryId,
            CanCheckWin = canCheckWin
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(Utils.UpdateCategoryBookUrl,  body);

        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return Utils.GetJsonPropertyAsBool(jsonResponse, new[] { "result" });
    }
}