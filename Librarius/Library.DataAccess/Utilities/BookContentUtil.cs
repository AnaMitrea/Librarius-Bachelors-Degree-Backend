using System.Text.RegularExpressions;
using Library.DataAccess.DTOs;

namespace Library.DataAccess.Utilities;

public partial class BookContentUtil
{
    public static async Task<string> GetContentBetweenSectionsAsync(string html)
    {
        // search for the start and end section IDs in the HTML
        var startSectionRegex = new Regex("<section\\s+class=\"pg-boilerplate\\spgheader\"\\s+id=\"pg-header\"\\s+lang=\"en\">([\\s\\S]*?)<\\/section>", RegexOptions.IgnoreCase);
        var endSectionRegex = new Regex("<section\\s+class=\"pg-boilerplate\\spgheader\"\\s+id=\"pg-footer\"\\s+lang=\"en\">", RegexOptions.IgnoreCase);
        var startMatch = startSectionRegex.Match(html);
        var endMatch = endSectionRegex.Match(html);

        if (!startMatch.Success || !endMatch.Success || endMatch.Index <= startMatch.Index)
        {
            throw new Exception("Could not find valid start and end sections");
        }

        // extract the content between the start and end sections
        var startIndex = startMatch.Index + startMatch.Length;
        var endIndex = endMatch.Index;
        var content = html.Substring(startIndex, endIndex - startIndex);

        // remove the start and end sections from the content
        content = content.Replace(startMatch.Value, "").Replace(endMatch.Value, "");
        return content;
    }
    
    public static int CountWords(string htmlContent)
    {
        var wordCount = 0;
        var currentIndex = 0;
        var inTag = false;

        while (currentIndex < htmlContent.Length)
        {
            var currentChar = htmlContent[currentIndex];

            switch (currentChar)
            {
                case '<':
                    inTag = true;
                    break;
                case '>':
                    inTag = false;
                    break;
                default:
                {
                    if (!inTag && !char.IsWhiteSpace(currentChar))
                    {
                        wordCount++;
                        
                        while (currentIndex < htmlContent.Length && !char.IsWhiteSpace(htmlContent[currentIndex]) && htmlContent[currentIndex] != '<')
                        {
                            currentIndex++;
                        }
                    }

                    break;
                }
            }

            currentIndex++;
        }

        return wordCount;
    }
    
    public static ReadingTimeResponseDto CalculateReadingTime(int wordCount)
    {
        const int wordsPerMinute = 200; // Average reading speed (words per minute)
        const int secondsPerMinute = 60;

        var minutes = wordCount / wordsPerMinute;
        var seconds = (int)Math.Round((double)wordCount / wordsPerMinute * secondsPerMinute) % secondsPerMinute;

        var hours = 0;
        if (minutes >= 60)
        {
            hours = minutes / 60;
            minutes %= 60;
        }

        var response = new ReadingTimeResponseDto
        {
            Hours = hours,
            Minutes = minutes,
            Seconds = seconds
        };

        return response;
    }
    
    public static string RemoveHtmlTags(string htmlContent)
    {
        var regex = new Regex("<[^>]+?>");
        return regex.Replace(htmlContent, "");
    }
    
    // [GeneratedRegex("<section\\s+class=\"pg-boilerplate\\spgheader\"\\s+id=\"pg-header\"\\s+lang=\"en\">([\\s\\S]*?)<\\/section>", RegexOptions.IgnoreCase, "en-GB")]
    // private static partial Regex PgHeaderClassRegex();
    // [GeneratedRegex("<section\\s+class=\"pg-boilerplate\\spgheader\"\\s+id=\"pg-footer\"\\s+lang=\"en\">", RegexOptions.IgnoreCase, "en-GB")]
    // private static partial Regex PgFooterClassRegex();
}