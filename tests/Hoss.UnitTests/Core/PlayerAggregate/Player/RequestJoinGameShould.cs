// 🃏 The HossGame 🃏
// <copyright file="RequestJoinGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.PlayerAggregate;
using Hoss.Core.PlayerAggregate.Events;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

#endregion

public class RequestJoinGameShould
{
    [Theory]
    [AutoPlayerData]
    public void RaiseRequestJoinGameEvent(Player player, AGameId gameId)
    {
        player.RequestJoinGame(gameId);

        player.Events.Should().ContainSingle(e => e is RequestedJoinGameEvent).Which.Should()
            .BeOfType<RequestedJoinGameEvent>().Which.GameId.Should().Be(gameId);
    }

    [Theory]
    [AutoPlayerData]
    public void RaiseCannotJoinGameEventWhenPlayerAlreadyInAGame(Player player, AGameId gameId, AGameId anotherGameId)
    {
        player.RequestJoinGame(gameId);
        player.RequestJoinGame(anotherGameId);

        var @event = player.Events.Should().ContainSingle(e => e is CannotJoinGameEvent);
        @event.Which.Should().BeOfType<CannotJoinGameEvent>();
        @event.Subject.Should().BeAssignableTo<PlayerEventBase>().Which.PlayerId.Should().Be(player.Id);
    }

    [Theory]
    [AutoPlayerData]
    public void ThrowInvalidEntityStateException(Player player, NoGameId noGameId)
    {
        var requestAction = () => player.RequestJoinGame(noGameId);

        requestAction.Should().Throw<InvalidEntityStateException>();
    }
}