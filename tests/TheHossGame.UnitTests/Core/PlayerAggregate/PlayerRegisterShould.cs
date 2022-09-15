// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.Services;
using Xunit;
using Events = TheHossGame.Core.PlayerAggregate.Events;

public class PlayerRegisterShould
{
    [Theory]
    [AutoPlayerData]
    [AutoMoqData]
    public void RaisePlayerRegisteredEvent(
        Player player)
    {
        player.Register();

        var @event = player.DomainEvents.Should().ContainSingle()
           .Subject.As<Events.PlayerRegisteredEvent>();
        @event = @event.Should().BeOfType<Events.PlayerRegisteredEvent>().Subject;
        @event.PlayerId.Should().NotBeNull();
        @event.PlayerName.Should().NotBeNull();
        @event.PlayerEmail.Should().NotBeNull();
    }
}
