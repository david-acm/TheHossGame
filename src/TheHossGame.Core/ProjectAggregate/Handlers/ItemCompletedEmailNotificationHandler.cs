// 🃏 The HossGame 🃏
// <copyright file="ItemCompletedEmailNotificationHandler.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.ProjectAggregate.Events;

public class ItemCompletedEmailNotificationHandler : INotificationHandler<ToDoItemCompletedEvent>
{
    private readonly IEmailSender emailSender;

    // In a REAL app you might want to use the Outbox pattern and a command/queue here...
    public ItemCompletedEmailNotificationHandler(IEmailSender emailSender)
    {
        this.emailSender = emailSender;
    }

    // configure a test email server to demo this works
    // https://ardalis.com/configuring-a-local-test-email-server
    public Task Handle(ToDoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));

        return this.emailSender.SendEmailAsync("test@test.com", "test@test.com", $"{notification.CompletedItem.Title} was completed.", notification.CompletedItem.ToString());
    }
}
