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
using Hoss.Core.GameAggregate.Events;
using Hoss.Core.Interfaces;
using Hoss.Core.PlayerAggregate;
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

        var newGameEvent = game.Events.Should().ContainSingle(e => e is NewGameCreatedEvent).Subject
            .As<NewGameCreatedEvent>();

        var joinedEvent = game.Events.Should().ContainSingle(e => e is PlayerJoinedEvent).Subject
            .As<PlayerJoinedEvent>();

        newGameEvent.StartedBy.Should().Be(playerId);
        newGameEvent.StartedBy.Should().BeOfType<APlayerId>();
        newGameEvent.Should().BeAssignableTo<GameEventBase>().Subject.GameId.Should().Be(game.Id);
        joinedEvent.PlayerId.Should().Be(playerId);
        joinedEvent.TeamId.Should().Be(Team1);
        joinedEvent.Should().BeAssignableTo<GameEventBase>();
    }
}