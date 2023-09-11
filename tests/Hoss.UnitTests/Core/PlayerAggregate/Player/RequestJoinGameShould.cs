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
using Hoss.SharedKernel;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

#endregion

public class RequestJoinGameShould
{
    [Theory]
    [PlayerData]
    public void RaiseRequestJoinGameEvent(Profile profile, AGameId gameId)
    {
        profile.RequestJoinGame(gameId);

        profile.Events.Should().ContainSingle(e => e is RequestedJoinGameEvent).Which.Should()
            .BeOfType<RequestedJoinGameEvent>().Which.GameId.Should().Be(gameId);
    }

    [Theory]
    [PlayerData]
    public void RaiseCannotJoinGameEventWhenPlayerAlreadyInAGame(Profile profile, AGameId gameId, AGameId anotherGameId)
    {
        profile.RequestJoinGame(gameId);
        profile.RequestJoinGame(anotherGameId);

        var @event = profile.Events.Should().ContainSingle(e => e is CannotJoinGameEvent);
        @event.Which.Should().BeOfType<CannotJoinGameEvent>();
        @event.Subject.Should().BeAssignableTo<ProfileEventBase>().Which.ProfileId.Should().Be(profile.Id);
    }

    [Theory]
    [PlayerData]
    public void ThrowInvalidEntityStateException(Profile profile, NoValueId noGameId)
    {
        var requestAction = () => profile.RequestJoinGame(noGameId);

        requestAction.Should().Throw<InvalidEntityStateException>();
    }
}