namespace Identity.Application.Models.Requests;

public class AuthenticationRequestModel
{
    public string Username { get; set; }
    
    public string Password { get; set; }
}