using System.Net;
using System.Net.Mail;
using Email.Application.Templates;
using Microsoft.Extensions.Configuration;

namespace Email.Application.Services.Implementations;

public class EmailSender : IEmailSender
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _emailAccount;
    private readonly string _emailPassword;
    
    public EmailSender(IConfiguration configuration)
    {
        _smtpHost = configuration["SmtpConfiguration:Host"] ?? throw new Exception("Could not configure SMTP server.");
        _smtpPort = Convert.ToInt32(configuration["SmtpConfiguration:Port"] ?? throw new Exception("Could not configure SMTP server."));
        _emailAccount = configuration["SmtpConfiguration:Email"] ?? throw new Exception("Could not configure SMTP server.");
        _emailPassword = configuration["SmtpConfiguration:Password"] ?? throw new Exception("Could not configure SMTP server.");
    }
    
    public Task SendEmailAsync(int authorId)
    {
        var client = new SmtpClient(_smtpHost, _smtpPort)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailAccount, _emailPassword)
        };

        var message = new MailMessage
        {
            From = new MailAddress(_emailAccount),
            To = { new MailAddress("anamitrea27@gmail.com") },
            Subject = "Author Subscription Confirmation",
            Body = AuthorSubscriptionTemplate.GetSubscriptionConfirmationEmailBody("Ana Banana"),
            IsBodyHtml = true
        };

        var msg = client.SendMailAsync(message);

        return msg;
    }
    
    
}