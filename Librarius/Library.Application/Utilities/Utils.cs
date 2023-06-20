using System.Globalization;
using System.Text.Json;
using Library.DataAccess.Entities.BookRelated;

namespace Library.Application.Utilities;

public static class Utils
{
    public const string TrophyRewardUrl = "http://localhost:5164/api/trophy/reward/check-win";
    public const string TrophyLengthyReviewRewardUrl = "http://localhost:5164/api/trophy/win/lengthy-review";
    public const string UpdateReadingTimeUrl = "http://localhost:5164/api/trophy/reading-time/reward/update-activity";
    public const string UpdateReadingBookUrl = "http://localhost:5164/api/trophy/reading-books/reward/update-activity";
    public const string UpdateCategoryBookUrl = "http://localhost:5164/api/trophy/category-reader/reward/update-activity";
    
    public static string CalculateTimeUnit(string timestamp)
    {
        if (DateTime.TryParseExact(timestamp, "dd/MM/yyyy", null, DateTimeStyles.None, out var timestampDate))
        {
            var timePassed = DateTime.Now - timestampDate;
            return timePassed.TotalDays switch
            {
                >= 365 => "years",
                >= 30 => "months",
                _ => "days"
            };
        }

        return " ";
    } 
    
    public static string CalculateTimeValue(string timestamp)
    {
        if (DateTime.TryParseExact(timestamp, "dd/MM/yyyy", null, DateTimeStyles.None, out var timestampDate))
        {
            var timePassed = DateTime.Now - timestampDate;
            switch (timePassed.TotalDays)
            {
                case >= 365:
                {
                    var years = (int)(timePassed.TotalDays / 365);
                    return years.ToString();
                }
                case >= 30:
                {
                    var months = (int)(timePassed.TotalDays / 30);
                    return months.ToString();
                }
                default:
                {
                    var days = (int)timePassed.TotalDays;
                    return days.ToString();
                }
            }
        }

        return " ";
    }
    
    public static int CalculateOverallRating(IEnumerable<Review> reviews)
    {
        var enumerable = reviews.ToList();
        if (!enumerable.Any())
        {
            return 0;
        }

        var totalRating = 0;
        var reviewCount = 0;

        foreach (var review in enumerable)
        {
            totalRating += review.Rating;
            reviewCount++;
        }

        var overallRating = reviewCount > 0 ? totalRating / reviewCount : 0;
        return overallRating;
    }
    
    public static string GetJsonPropertyAsString(string jsonResponse, IEnumerable<string> propertyPath)
    {
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var property = jsonDocument.RootElement;

        foreach (var propertyName in propertyPath)
        {
            if (property.ValueKind != JsonValueKind.Object || !property.TryGetProperty(propertyName, out property))
            {
                throw new Exception("Json Result property not found.");
            }
        }

        return property.GetString() ?? string.Empty;
    }
    
    public static bool GetJsonPropertyAsBool(string jsonResponse, IEnumerable<string> propertyPath)
    {
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var property = jsonDocument.RootElement;

        foreach (var propertyName in propertyPath)
        {
            if (property.ValueKind != JsonValueKind.Object || !property.TryGetProperty(propertyName, out property))
            {
                throw new Exception("Json Result property not found.");
            }
        }

        return property.GetBoolean();
    }
    
    public static int GetJsonPropertyAsInteger(string jsonResponse, IEnumerable<string> propertyPath)
    {
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var property = jsonDocument.RootElement;

        foreach (var propertyName in propertyPath)
        {
            if (property.ValueKind != JsonValueKind.Object || !property.TryGetProperty(propertyName, out property))
            {
                throw new Exception("Json Result property not found.");
            }
        }
        return property.GetInt32();
    }
}