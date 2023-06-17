namespace Email.Application.Services;

public interface IEmailSender
{
    Task SendAuthorSubscriptionEmailAsync(int authorId, string token);

    Task SendWelcomeEmailAsync(string token);
}