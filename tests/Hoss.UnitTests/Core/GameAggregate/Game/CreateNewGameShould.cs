// 🃏 The HossGame 🃏
// <copyright file="CreateNewGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using FluentAssertions;

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using Hoss.Core.GameAggregate;
using Hoss.Core.Interfaces;
using PlayerAggregate.Generators;
using Xunit;

#endregion

public class CreateNewGameShould
{
    [Theory]
    [PlayerData]
    public void RaiseGameStartedEvent(APlayerId playerId, IShufflingService shuffleService)
    {
        var game = AGame.CreateForPlayer(new AGameId(Guid.NewGuid()), playerId, shuffleService);

        var newGameEvent = game.Events.Should().ContainSingle(e => e is GameEvents.NewGameCreatedEvent).Subject
            .As<GameEvents.NewGameCreatedEvent>();

        var joinedEvent = game.Events.Should().ContainSingle(e => e is GameEvents.PlayerJoinedEvent).Subject
            .As<GameEvents.PlayerJoinedEvent>();

        newGameEvent.StartedBy.Should().Be(playerId);
        newGameEvent.StartedBy.Should().BeOfType<APlayerId>();
        newGameEvent.Should().BeAssignableTo<GameEvents.GameEventBase>().Subject.GameId.Id.Should().Be(game.Id);
        joinedEvent.PlayerId.Id.Should().Be(playerId.Id);
        joinedEvent.TeamId.Should().Be(TeamId.NorthSouth);
        joinedEvent.Should().BeAssignableTo<GameEvents.GameEventBase>();
    }
}