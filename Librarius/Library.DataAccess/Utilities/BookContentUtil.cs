using System.Text.RegularExpressions;

namespace Library.DataAccess.Utilities;

public partial class BookContentUtil
{
    public static async Task<string> GetContentBetweenSectionsAsync(string html)
    {
        // search for the start and end section IDs in the HTML
        var startSectionRegex = PgHeaderClassRegex();
        var endSectionRegex = PgFooterClassRegex();
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

    [GeneratedRegex("<section\\s+class=\"pg-boilerplate\\spgheader\"\\s+id=\"pg-header\"\\s+lang=\"en\">([\\s\\S]*?)<\\/section>", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex PgHeaderClassRegex();
    [GeneratedRegex("<section\\s+class=\"pg-boilerplate\\spgheader\"\\s+id=\"pg-footer\"\\s+lang=\"en\">", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex PgFooterClassRegex();
}