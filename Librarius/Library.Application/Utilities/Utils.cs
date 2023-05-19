using Library.DataAccess.Entities.BookRelated;

namespace Library.Application.Utilities;

public static class Utils
{
    public static string CalculateTimeUnit(string timestamp)
    {
        if (DateTime.TryParseExact(timestamp, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var timestampDate))
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
        if (DateTime.TryParseExact(timestamp, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var timestampDate))
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
}