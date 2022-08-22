// 🃏 The HossGame 🃏
// <copyright file="EmailSender.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure;

using System.Net.Mail;
using Microsoft.Extensions.Logging;
using TheHossGame.Core.Interfaces;
using TheHossGame.Infrastructure.Logging;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        this.logger = logger;
    }

    public async Task SendEmailAsync(string destinatary, string from, string subject, string body)
    {
        using var emailClient = new SmtpClient("localhost");
        using var message = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            Body = body,
        };

        message.To.Add(new MailAddress(destinatary));
        this.logger.LogWarning(string.Empty);
        await emailClient.SendMailAsync(message);
    }
}
