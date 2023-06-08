using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Library.API.Utils;

public static class Utilities
{
    public static string ExtractUsernameFromAccessToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var username = jwtSecurityToken.Claims.First(claim => claim.Type == "name").Value;

        return username;
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