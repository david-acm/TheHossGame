// 🃏 The HossGame 🃏
// <copyright file="PlayerReadyShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static TheHossGame.Core.GameAggregate.AGame.GameState;

public class PlayerReadyShould
{
   [Theory]
   [AutoReadyGameData]
   public void RaiseStartEvent([Frozen] AGame game)
   {
      var (gameId, roundId, _) = game.Events.ShouldContain()
                                     .SingleEventOfType<RoundStartedEvent>();

      gameId.Should().Be(game.Id);
      roundId.Should().NotBeNull();
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseGameStartedEventWhenAllPlayersHaveJoined(AGame readyGame)
   {
      readyGame.Events.ShouldContain()
               .ManyEventsOfType<PlayerReadyEvent>(4);
      readyGame.Events.ShouldContain()
               .SingleEventOfType<TeamsFormedEvent>();
      var startedEvent = readyGame.Events.ShouldContain()
                                  .SingleEventOfType<GameStartedEvent>();

      startedEvent.Should().NotBeNull();
      startedEvent.GameId.Should().Be(readyGame.Id);
      readyGame.State.Should().Be(Started);
   }

   [Theory]
   [AutoPlayerData]
   public void RaisePlayerReadyEventWhenPlayerIsMember(
      [Frozen] PlayerId playerId,
      AGame game)
   {
      game.TeamPlayerReady(playerId);

      var readyEvent = game.Events
                           .ShouldContain()
                           .SingleEventOfType<PlayerReadyEvent>();

      readyEvent.Should().NotBeNull();
      readyEvent.PlayerId.Should().Be(playerId);
      readyEvent.GameId.Should().Be(game.Id);
      game.FindPlayer(playerId).IsReady
          .Should().BeTrue();
      game.State.Should().Be(Created);
   }

   [Theory]
   [AutoPlayerData]
   public void NotRaisePlayerReadyEventWhenPlayerIsNotMember(
      PlayerId playerId,
      AGame game)
   {
      game.TeamPlayerReady(playerId);

      game.Events
          .Where(e => e is PlayerReadyEvent)
          .Should().BeEmpty();

      game.FindPlayer(playerId).IsReady
          .Should().BeFalse();
   }
}
