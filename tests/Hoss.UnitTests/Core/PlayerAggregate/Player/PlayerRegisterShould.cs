// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

#region

using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.PlayerEntity;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.PlayerAggregate;
using Hoss.Core.PlayerAggregate.Events;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

#endregion

public class RegisterShould
{
    [Theory]
    [AutoPlayerData]
    public void RaisePlayerRegisteredEvent(APlayerId playerId, PlayerName playerName)
    {
        var player = Player.FromRegister(playerId, playerName);

        var @event = player.Events.Should().ContainSingle().Subject.As<PlayerRegisteredEvent>();
        @event = @event.Should().BeOfType<PlayerRegisteredEvent>().Subject;
        @event.PlayerId.Should().NotBeNull();
        @event.PlayerName.Should().NotBeNull();
    }

    [Theory]
    [AutoPlayerData]
    public void BeEqualById(
        [Frozen] PlayerId playerId,
        NoGamePlayer gamePlayer1,
        NoGamePlayer gamePlayer2)
    {
        gamePlayer1.Should().Be(gamePlayer2);
        gamePlayer1.PlayerId.Should().Be(playerId);
    }

    [Theory]
    [AutoPlayerData]
    public void NotBeEqualByType(
        AGame game,
        NoRound noRound,
        Player player,
        NoBase noBase)
    {
        game.Should().NotBe(noRound);
        player.Should().NotBe(noBase);
        game.Should().Be(game);
    }
}