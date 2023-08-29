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
using Hoss.Core.GameAggregate.Events;
using Hoss.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static Hoss.Core.GameAggregate.Game.TeamId;

#endregion

public class JoinGameShould
{
    [Theory]
    [AutoPlayerData]
    public void RaisePlayerAlreadyInGame([Frozen] PlayerId playerId, AGame game)
    {
        game.JoinPlayerToTeam(playerId, Team1);

        game.Events.ShouldContain().SingleEventOfType<PlayerAlreadyInGameEvent>().PlayerId.Should().Be(playerId);
    }

    [Theory]
    [AutoPlayerData]
    public void RaiseTeamsFormedEvent(IEnumerable<Player> players, AGame game)
    {
        var playerList = players.ToList();
        game.JoinPlayerToTeam(playerList[0].Id, Team1);
        game.JoinPlayerToTeam(playerList[1].Id, Team2);
        game.JoinPlayerToTeam(playerList[2].Id, Team2);

        var @event = game.Events.ShouldContain().SingleEventOfType<TeamsFormedEvent>();
        @event.GameId.Should().Be(game.Id);
        @event.Should().BeAssignableTo<GameEventBase>().Subject.GameId.Should().Be(game.Id);
    }

    [Theory]
    [AutoPlayerData]
    public void RaisePlayerJoinedEvent(Player player, AGame game)
    {
        game.JoinPlayerToTeam(player.Id, Team1);

        game.Events.Where(e => e is PlayerJoinedEvent).Should().HaveCount(2);
    }

    [Theory]
    [AutoPlayerData]
    public void ThrowInvalidEntityStateWhenTeamsAreComplete(IEnumerable<PlayerId> playerIds, AGame game)
    {
        var playerIdList = playerIds.ToList();
        game.JoinPlayerToTeam(playerIdList[0], Team1);
        var joinPlayerAction = () => game.JoinPlayerToTeam(playerIdList[1], Team1);

        joinPlayerAction.Should().Throw<InvalidEntityStateException>();
    }
}