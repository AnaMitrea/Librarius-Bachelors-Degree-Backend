namespace Identity.Application.Models;

public class AuthenticationResponseModel
{
    public string Username { get; set; }
    
    public string JwtToken { get; set; }
    
    public int ExpiresIn { get; set; }
}