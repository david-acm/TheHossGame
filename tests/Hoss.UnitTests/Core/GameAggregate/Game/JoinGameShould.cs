// 🃏 The HossGame 🃏
// <copyright file="JoinGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static Hoss.Core.GameAggregate.Game.TeamId;
using static Hoss.Core.GameAggregate.GameEvents;

#endregion

public class JoinGameShould
{
    [Theory]
    [PlayerData]
    public void RaisePlayerAlreadyInGame([Frozen] PlayerId playerId, AGame game)
    {
        game.JoinPlayerToTeam(playerId, Team1);

        game.Events.ShouldContain().SingleEventOfType<PlayerAlreadyInGameEvent>().PlayerId.Should().Be(playerId);
    }

    [Theory]
    [PlayerData]
    public void RaiseTeamsFormedEvent(IEnumerable<PlayerId> playerIds, AGame game)
    {
        var playerIdList = playerIds.ToList();
        game.JoinPlayerToTeam(playerIdList[0], Team1);
        game.JoinPlayerToTeam(playerIdList[1], Team2);
        game.JoinPlayerToTeam(playerIdList[2], Team2);

        var @event = game.Events.ShouldContain().SingleEventOfType<TeamsFormedEvent>();
        @event.GameId.Should().Be(game.Id);
        @event.Should().BeAssignableTo<GameEventBase>().Subject.GameId.Should().Be(game.Id);
    }

    [Theory]
    [PlayerData]
    public void RaisePlayerJoinedEvent(PlayerId playerId, AGame game)
    {
        game.JoinPlayerToTeam(playerId, Team1);

        game.Events.Where(e => e is PlayerJoinedEvent).Should().HaveCount(2);
    }

    [Theory]
    [PlayerData]
    public void ThrowInvalidEntityStateWhenTeamsAreComplete(IEnumerable<PlayerId> playerIds, AGame game)
    {
        var playerIdList = playerIds.ToList();
        game.JoinPlayerToTeam(playerIdList[0], Team1);
        var joinPlayerAction = () => game.JoinPlayerToTeam(playerIdList[1], Team1);

        joinPlayerAction.Should().Throw<InvalidEntityStateException>();
    }
}