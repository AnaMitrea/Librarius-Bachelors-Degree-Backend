namespace Email.Application.Services;

public interface IEmailSender
{
    Task SendEmailAsync(int authorId);
}