// 🃏 The HossGame 🃏
// <copyright file="DomainEventDispatcherShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using FluentAssertions;

namespace TheHossGame.UnitTests.SharedKernel;

#region

using AutoFixture.Xunit2;
using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;
using MediatR;
using Moq;
using Core.PlayerAggregate.Generators;
using Core.Services;
using Xunit;

#endregion

public class DomainEventDispatcherShould
{
    [Theory]
    [ReadyGameData]
    [AutoMoqData]
    public async Task ShouldPublishEventsAsync([Frozen] Mock<IMediator> mediator, AGame game,
        DomainEventDispatcher<AGame> dispatcher)
    {
        await dispatcher.DispatchAndClearEvents(new[] {game});

        var eventCount = game.Events.Count();
        mediator.Verify(m => m.Publish(It.IsAny<GameEvents.GameEventBase>(), default!), Times.Exactly(eventCount));
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