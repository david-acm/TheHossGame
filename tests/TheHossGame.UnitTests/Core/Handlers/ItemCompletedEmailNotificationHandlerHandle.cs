// 🃏 The HossGame 🃏
// <copyright file="ItemCompletedEmailNotificationHandlerHandle.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Handlers;

using Moq;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Events;
using TheHossGame.Core.ProjectAggregate.Handlers;
using Xunit;

public class ItemCompletedEmailNotificationHandlerHandle
{
    private readonly ItemCompletedEmailNotificationHandler handler;
    private readonly Mock<IEmailSender> emailSenderMock;

    public ItemCompletedEmailNotificationHandlerHandle()
    {
        this.emailSenderMock = new Mock<IEmailSender>();
        this.handler = new ItemCompletedEmailNotificationHandler(this.emailSenderMock.Object);
    }

    [Fact]
    public async Task ThrowsExceptionGivenNullEventArgument()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this.handler.Handle(null!, CancellationToken.None));
    }

    [Fact]
    public async Task SendsEmailGivenEventInstance()
    {
        await this.handler.Handle(new ToDoItemCompletedEvent(new ToDoItem()), CancellationToken.None);

        this.emailSenderMock.Verify(sender => sender.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
