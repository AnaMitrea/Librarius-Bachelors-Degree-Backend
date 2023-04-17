using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Models;
using Identity.Application.Models.Requests;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application;

public class JwtTokenHandler
{
    public const string JWT_SECURITY_KEY = "sajfkafbnebrkgbT%kcba82CffskgrhtgaBGDS9f71anflgbgvbvfe";

    private const int JWT_TOKEN_VALIDITY_MINUTES = 30;

    // hardcoded users for now
    // TODO get them from db after making the auth work

    private readonly List<UserAccount> _userAccounts;

    public JwtTokenHandler()
    {
        _userAccounts = new List<UserAccount>
        {
            new() { Username = "admin", Password = "admin", Role = "Administrator" },
            new() { Username = "ana", Password = "maria", Role = "User" }
        };
    }

    // TODO break the method into smaller ones
    public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authRequest)
    {
        if (string.IsNullOrEmpty(authRequest.Username) ||
            string.IsNullOrEmpty(authRequest.Password))
            return null;
        
        // TODO Validate username and password from DB
        
        var userAccount = _userAccounts.Find(x => x.Username == authRequest.Username && x.Password == authRequest.Password);
        if (userAccount == default) return null;

        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINUTES);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, authRequest.Username),
            new("Role", userAccount.Role)
        });

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature
        );

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new AuthenticationResponse
        {
            Username = userAccount.Username,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
            JwtToken = token
        };
    }
}