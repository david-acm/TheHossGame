// 🃏 The HossGame 🃏
// <copyright file="JoinGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using TheHossGame.UnitTests.Extensions;
using static Hoss.Core.GameAggregate.TeamId;

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;
using PlayerAggregate.Generators;
using Xunit;
using static Hoss.Core.GameAggregate.GameEvents;

#endregion

public class JoinGameShould
{
    [Theory]
    [PlayerData]
    public void RaisePlayerAlreadyInGame([Frozen] PlayerId playerId, AGame game)
    {
        game.JoinPlayerToTeam(playerId, NorthSouth);

        game.Events.ShouldContain().SingleEventOfType<PlayerAlreadyInGameEvent>().PlayerId.Should().Be(playerId);
    }

    [Theory]
    [PlayerData]
    public void RaiseTeamsFormedEvent(IEnumerable<PlayerId> playerIds, AGame game)
    {
        var playerIdList = playerIds.ToList();
        game.JoinPlayerToTeam(playerIdList[0], NorthSouth);
        game.JoinPlayerToTeam(playerIdList[1], EastWest);
        game.JoinPlayerToTeam(playerIdList[2], EastWest);

        var @event = game.Events.ShouldContain().SingleEventOfType<TeamsFormedEvent>();
        @event.GameId.Id.Should().Be(game.Id);
        @event.Should().BeAssignableTo<GameEventBase>().Subject.GameId.Id.Should().Be(game.Id);
    }

    [Theory]
    [PlayerData]
    public void RaisePlayerJoinedEvent(PlayerId playerId, AGame game)
    {
        game.JoinPlayerToTeam(playerId, NorthSouth);

        game.Events.Where(e => e is PlayerJoinedEvent).Should().HaveCount(2);
    }

    [Theory]
    [PlayerData]
    public void ThrowInvalidEntityStateWhenTeamsAreComplete(IEnumerable<PlayerId> playerIds, AGame game)
    {
        var playerIdList = playerIds.ToList();
        game.JoinPlayerToTeam(playerIdList[0], NorthSouth);
        var joinPlayerAction = () => game.JoinPlayerToTeam(playerIdList[1], NorthSouth);

        joinPlayerAction.Should().Throw<InvalidEntityStateException>();
    }
}