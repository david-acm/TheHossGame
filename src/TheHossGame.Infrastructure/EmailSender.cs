// 🃏 The HossGame 🃏
// <copyright file="EmailSender.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure;

using Microsoft.Extensions.Logging;
using System.Net.Mail;
using TheHossGame.Core.Interfaces;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        this.logger = logger;
    }

    public async Task SendEmailAsync(string addressee, string from, string subject, string body)
    {
        using var emailClient = new SmtpClient("localhost");
        using var message = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            Body = body,
        };

        message.To.Add(new MailAddress(addressee));
        this.logger.LogWarning(string.Empty);
        await emailClient.SendMailAsync(message);
    }
}
