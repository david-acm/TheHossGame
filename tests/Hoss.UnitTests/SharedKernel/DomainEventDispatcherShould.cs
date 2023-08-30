// 🃏 The HossGame 🃏
// <copyright file="DomainEventDispatcherShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.SharedKernel;

#region

using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.Events;
using Hoss.SharedKernel;
using MediatR;
using Moq;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Core.Services;
using Xunit;

#endregion

public class DomainEventDispatcherShould
{
    [Theory]
    [ReadyGameData]
    [AutoMoqData]
    public void ShouldPublishEventsAsync([Frozen] Mock<IMediator> mediator, AGame game,
        DomainEventDispatcher<GameId> dispatcher)
    {
        dispatcher.DispatchAndClearEvents(new[] {game});

        var eventCount = game.Events.Count();
        mediator.Verify(m => m.Publish(It.IsAny<GameEventBase>(), default!), Times.Exactly(eventCount));
        game.Events.Should().BeEmpty();
    }
}