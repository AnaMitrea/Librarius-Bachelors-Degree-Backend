using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Application.Models.Requests;
using Identity.Application.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services.Implementations;

public class JwtTokenHandlerService : IJwtTokenHandlerService
{
    // TODO read these from a config file!!!!
    public const string JWT_SECURITY_KEY = "sajfkafbnebrkgbT%kcba82CffskgrhtgaBGDS9f71anflgbgvbvfe";

    private readonly IAccountService _accountService;

    public JwtTokenHandlerService(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<AuthenticationResponseModel?> AuthenticateAccount(AuthenticationRequestModel authRequest)
    {
        if (string.IsNullOrEmpty(authRequest.Username) ||
            string.IsNullOrEmpty(authRequest.Password))
            return null;
        
        var userAccount = await _accountService.GetAccountAsync(authRequest.Username, authRequest.Password);
        
        return userAccount == default ? null : CreateResponseWithToken(authRequest.Username, userAccount.Role);
    }

    public async Task<RegisterUserAccountModel> RegisterAccount(RegisterRequestModel registerRequest)
    {
        if (string.IsNullOrEmpty(registerRequest.Username) ||
            string.IsNullOrEmpty(registerRequest.Password) ||
            string.IsNullOrEmpty(registerRequest.RePassword) ||
            string.IsNullOrEmpty(registerRequest.Email))
            throw new InvalidParametersException();
       
        if (string.CompareOrdinal(registerRequest.RePassword, registerRequest.Password) != 0)
            throw new Exception("Passwords don't match.");
        
        await _accountService.CheckUsernameExistence(registerRequest.Username);
        await _accountService.CheckEmailExistence(registerRequest.Email);
        
        return await _accountService.CreateAccountAsync(registerRequest);
    }

    private static AuthenticationResponseModel CreateResponseWithToken(string username, string role)
    {
        var claimsIdentity = ConfigureClaims(username, role);
        var signingCredentials = ConfigureSigningCredentials();
        
        var tokenExpiryTimeStamp = DateTime.Now.AddDays(1);
        
        var token = ConfigureJwtToken(claimsIdentity, signingCredentials, tokenExpiryTimeStamp);

        return new AuthenticationResponseModel
        {
            Username = username,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
            JwtToken = token
        };
    }
    
    private static AuthJwtResponseModel CreateJwtOnlyResponse(string username, string role)
    {
        var claimsIdentity = ConfigureClaims(username, role);
        var signingCredentials = ConfigureSigningCredentials();
        
        var tokenExpiryTimeStamp = DateTime.Now.AddDays(1);
        
        var token = ConfigureJwtToken(claimsIdentity, signingCredentials, tokenExpiryTimeStamp);

        return new AuthJwtResponseModel { JwtToken = token };
    }

    private static ClaimsIdentity ConfigureClaims(string username, string role)
    {
        return new ClaimsIdentity(new List<Claim> {
            new(JwtRegisteredClaimNames.Name, username),
            new("Role", role)
        });
    }

    private static SigningCredentials ConfigureSigningCredentials()
    {
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        
        return new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature
        );
    }

    private static string ConfigureJwtToken(ClaimsIdentity claimsIdentity, SigningCredentials signingCredentials, DateTime tokenExpiryTimeStamp)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };
        
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        return jwtSecurityTokenHandler.WriteToken(securityToken);
    }
}