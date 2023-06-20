using Email.Application.Models;

namespace Email.Application.Services;

public interface IEmailSender
{
    Task SendAuthorSubscriptionEmailAsync(int authorId, string token);

    Task SendWelcomeEmailAsync(WelcomeEmailRequest request);
}