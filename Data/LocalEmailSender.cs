using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

public class LocalEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        File.AppendAllText("email_log.txt", $"{DateTime.Now}: {email} - {subject}\n{htmlMessage}\n\n");

        System.Diagnostics.Debug.WriteLine("=== EMAIL SENT ===");
        System.Diagnostics.Debug.WriteLine($"To: {email}");
        System.Diagnostics.Debug.WriteLine($"Subject: {subject}");
        System.Diagnostics.Debug.WriteLine($"Message: {htmlMessage}");
        return Task.CompletedTask;
    }

}
