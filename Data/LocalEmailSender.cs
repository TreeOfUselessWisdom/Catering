using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;

public class LocalEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // write reset link to console
        Console.WriteLine($"\n--- Reset email to {email} ---");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine(htmlMessage);
        Console.WriteLine($"--- end reset email ---\n");
        return Task.CompletedTask;
    }
}
