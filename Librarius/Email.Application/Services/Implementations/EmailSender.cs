using System.Net;
using System.Net.Http.Headers;
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

    private const string UserEmailRequestUrl = "http://localhost:5164/api/user/email";

    public EmailSender(IConfiguration configuration)
    {
        _smtpHost = configuration["SmtpConfiguration:Host"] ?? throw new Exception("Could not configure SMTP server.");
        _smtpPort = Convert.ToInt32(configuration["SmtpConfiguration:Port"] ?? throw new Exception("Could not configure SMTP server."));
        _emailAccount = configuration["SmtpConfiguration:Email"] ?? throw new Exception("Could not configure SMTP server.");
        _emailPassword = configuration["SmtpConfiguration:Password"] ?? throw new Exception("Could not configure SMTP server.");
    }
    
    public async Task SendEmailAsync(int authorId, string token)
    {
        var authorNameResponse = await GetAuthorNameAsync(authorId, token);
        var userEmailResponse = await GetUserEmailAsync(token);

        await SendSubscriptionConfirmationEmailAsync(authorNameResponse, userEmailResponse);
    }

    private async Task<string> GetAuthorNameAsync(int authorId, string token)
    {
        using var httpClient = new HttpClient();
        var authorUrl = $"http://localhost:5164/api/library/author/{authorId}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var authorResponse = await httpClient.GetAsync(authorUrl);
        authorResponse.EnsureSuccessStatusCode();
        return await authorResponse.Content.ReadAsStringAsync();
    }

    private async Task<string> GetUserEmailAsync(string token)
    {
        using var httpClient = new HttpClient();
       
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var userResponse = await httpClient.GetAsync(UserEmailRequestUrl);
        userResponse.EnsureSuccessStatusCode();
        return await userResponse.Content.ReadAsStringAsync();
    }

    private async Task SendSubscriptionConfirmationEmailAsync(string authorName, string userEmail)
    {
        using var client = new SmtpClient(_smtpHost, _smtpPort);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailAccount, _emailPassword);

        var message = new MailMessage
        {
            From = new MailAddress(_emailAccount),
            To = { new MailAddress(userEmail) },
            Subject = "Author Subscription Confirmation",
            Body = AuthorSubscriptionTemplate.GetSubscriptionConfirmationEmailBody(authorName),
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}