// 🃏 The HossGame 🃏
// <copyright file="CreateNewGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;
using static TheHossGame.Core.GameAggregate.Game.TeamId;

public class CreateNewGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaiseGameStartedEvent(
      APlayerId playerId,
      IShufflingService shuffleService)
   {
      var game = AGame.CreateForPlayer(playerId, shuffleService);

      var (_, startedBy) = game.Events.Should()
                               .ContainSingle(e => e is NewGameCreatedEvent)
                               .Subject.As<NewGameCreatedEvent>();

      var joinedEvent = game.Events.Should()
                            .ContainSingle(e => e is PlayerJoinedEvent)
                            .Subject.As<PlayerJoinedEvent>();

      startedBy.Should().Be(playerId);
      startedBy.Should().BeOfType<APlayerId>();
      joinedEvent.PlayerId.Should().Be(playerId);
      joinedEvent.TeamId.Should().Be(Team1);
   }
}
