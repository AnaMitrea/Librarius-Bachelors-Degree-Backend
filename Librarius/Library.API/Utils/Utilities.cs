using System.IdentityModel.Tokens.Jwt;

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
}