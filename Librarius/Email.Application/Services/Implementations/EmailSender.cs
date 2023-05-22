using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json;
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
    
    public async Task SendEmailAsync(int authorId, string token)
    {
        var authorNameResponse = await GetAuthorNameAsync(authorId, token);
        var userEmailResponse = await GetUserEmailAsync(token);

        await SendSubscriptionConfirmationEmailAsync(authorNameResponse, userEmailResponse);
    }

    private async Task<string> GetAuthorNameAsync(int authorId, string token)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var authorUrl = $"http://localhost:5164/api/library/author/{authorId}";

        var authorResponse = await httpClient.GetAsync(authorUrl);
        authorResponse.EnsureSuccessStatusCode();
        
        var jsonResponse = await authorResponse.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var root = jsonDocument.RootElement;
        var resultProperty = root.GetProperty("result");
        var nameProperty = resultProperty.GetProperty("name");
        var name = nameProperty.GetString();

        return name ?? throw new InvalidOperationException();
    }

    private async Task<string> GetUserEmailAsync(string token)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var userResponse = await httpClient.GetAsync(UserEmailRequestUrl);
        userResponse.EnsureSuccessStatusCode();
    
        var jsonResponse = await userResponse.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var root = jsonDocument.RootElement;
        var resultProp = root.GetProperty("result");
        var email = resultProp.GetString();
    
        return email ?? throw new InvalidOperationException();
    }
    
    // private async Task<string> GetUserEmailAsync(string token)
    // {
    //     using var httpClient = new HttpClient();
    //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     
    //     var userResponse = await httpClient.GetAsync(UserEmailRequestUrl);
    //     userResponse.EnsureSuccessStatusCode();
    //     
    //     var userEmail = await Utilities.GetPropertyValueAsync<string>(userResponse, "result.email");
    //     return userEmail;
    // }

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