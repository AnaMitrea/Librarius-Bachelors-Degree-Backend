using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using Email.Application.Models;
using Email.Application.Templates;
using Email.Application.Utils;
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
    
    public async Task SendAuthorSubscriptionEmailAsync(int authorId, string token)
    {
        var authorNameResponse = await GetAuthorNameAsync(authorId, token);
        var userInfo = await GetUserInfoAsync(token);

        await SendSubscriptionConfirmationEmailAsync(authorNameResponse, userInfo.Email);
    }
    
    public async Task SendWelcomeEmailAsync(WelcomeEmailRequest request)
    {
        await SendWelcomeConfirmationEmailAsync(request.Username, request.Email);
    }

    private async Task<string> GetAuthorNameAsync(int authorId, string token)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var authorUrl = $"http://localhost:5164/api/library/author/{authorId}";

        var authorResponse = await httpClient.GetAsync(authorUrl);
        authorResponse.EnsureSuccessStatusCode();
        
        var jsonResponse = await authorResponse.Content.ReadAsStringAsync();

        var name = Utilities.GetJsonPropertyAsString(jsonResponse, new[] { "result", "name" });

        return name ?? throw new InvalidOperationException();
    }

    private async Task<UserModel> GetUserInfoAsync(string token)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var userResponse = await httpClient.GetAsync(UserEmailRequestUrl);
        userResponse.EnsureSuccessStatusCode();
    
        var jsonResponse = await userResponse.Content.ReadAsStringAsync();
        var email = Utilities.GetJsonPropertyAsString(jsonResponse, new[] { "result", "email" });
        var username = Utilities.GetJsonPropertyAsString(jsonResponse, new[] { "result", "username" });
    
        return new UserModel
        {
            Email = email,
            Username = username
        };
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
    
    private async Task SendWelcomeConfirmationEmailAsync(string username, string userEmail)
    {
        using var client = new SmtpClient(_smtpHost, _smtpPort);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailAccount, _emailPassword);

        var message = new MailMessage
        {
            From = new MailAddress(_emailAccount),
            To = { new MailAddress(userEmail) },
            Subject = "Welcome to Librarius",
            Body = AccountConfirmationTemplate.GetConfirmationEmailBody(username),
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}