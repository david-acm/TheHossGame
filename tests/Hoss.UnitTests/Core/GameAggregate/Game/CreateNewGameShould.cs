// 🃏 The HossGame 🃏
// <copyright file="CreateNewGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.Interfaces;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;
using static Hoss.Core.GameAggregate.Game.TeamId;

#endregion

public class CreateNewGameShould
{
    [Theory]
    [PlayerData]
    public void RaiseGameStartedEvent(APlayerId playerId, IShufflingService shuffleService)
    {
        var game = AGame.CreateForPlayer(playerId, shuffleService);

        var newGameEvent = game.Events.Should().ContainSingle(e => e is GameEvents.NewGameCreatedEvent).Subject
            .As<GameEvents.NewGameCreatedEvent>();

        var joinedEvent = game.Events.Should().ContainSingle(e => e is GameEvents.PlayerJoinedEvent).Subject
            .As<GameEvents.PlayerJoinedEvent>();

        newGameEvent.StartedBy.Should().Be(playerId);
        newGameEvent.StartedBy.Should().BeOfType<APlayerId>();
        newGameEvent.Should().BeAssignableTo<GameEvents.GameEventBase>().Subject.GameId.Should().Be(game.Id);
        joinedEvent.PlayerId.Should().Be(playerId);
        joinedEvent.TeamId.Should().Be(Team1);
        joinedEvent.Should().BeAssignableTo<GameEvents.GameEventBase>();
    }
}