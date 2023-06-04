using System.Text.Json;

namespace Email.Application.Utils;

public static class Utilities
{
    public static string? GetJsonProperty(string jsonResponse, IEnumerable<string> propertyPath)
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
        
        return property.GetString();
    }
}