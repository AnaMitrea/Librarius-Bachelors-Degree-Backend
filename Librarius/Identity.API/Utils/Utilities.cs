﻿using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Identity.API.Utils;

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
    
    // valentine || christmas || weekend
    public static string CheckDate(DateTime date)
    {
        return date.Month switch
        {
            2 when date.Day == 14 => "valentine",
            12 when date.Day == 25 => "christmas",
            _ => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday ? "weekend" : string.Empty
        };
    }
}