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
    public async Task ShouldPublishEventsAsync([Frozen] Mock<IMediator> mediator, AGame game,
        DomainEventDispatcher<Game> dispatcher)
    {
        await dispatcher.DispatchAndClearEvents(new[] {game});

        var eventCount = game.Events.Count();
        mediator.Verify(m => m.Publish(It.IsAny<GameEventBase>(), default!), Times.Exactly(eventCount));
        game.Events.Should().BeEmpty();
    }

    // public void Test()
    // {
    //     this.ShouldPublishEventsAsync(default!, default!, new DomainEventDispatcher<Game>(default!));
    //
    //     IDomainEventDispatcher<AGame> dispatcher = new 
    //         DomainEventDispatcher<Game>(default!);
    // }
}