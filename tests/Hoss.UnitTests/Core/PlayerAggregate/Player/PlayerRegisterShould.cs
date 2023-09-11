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
    [PlayerData]
    public void RaisePlayerRegisteredEvent(ProfileEmail email, PlayerName name)
    {
        var player = Profile.FromNewRegister(email, name);

        var @event = player.Events.Should().ContainSingle().Subject.As<PlayerRegisteredEvent>();
        @event = @event.Should().BeOfType<PlayerRegisteredEvent>().Subject;
        @event.PlayerId.Should().NotBeNull();
        @event.PlayerName.Should().NotBeNull();
    }

    [Theory]
    [PlayerData]
    public void BeEqualById(
        [Frozen] PlayerId playerId,
        NoGamePlayer gamePlayer1,
        NoGamePlayer gamePlayer2)
    {
        gamePlayer1.Should().Be(gamePlayer2);
        gamePlayer1.PlayerId.Should().Be(playerId);
    }

    [Theory]
    [PlayerData]
    public void NotBeEqualByType(
        AGame game,
        NoRound noRound,
        Profile profile,
        NoBase noBase)
    {
        game.Should().NotBe(noRound);
        profile.Should().NotBe(noBase);
        game.Should().Be(game);
    }
}