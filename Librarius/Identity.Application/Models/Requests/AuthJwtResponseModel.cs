namespace Identity.Application.Models.Requests;

public class AuthJwtResponseModel
{
    public string JwtToken { get; set; }

    public bool HasWon { get; set; } = false;
}