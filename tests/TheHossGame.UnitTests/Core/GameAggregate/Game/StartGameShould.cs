// 🃏 The HossGame 🃏
// <copyright file="StartGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate;
using Xunit;

public class StartGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaiseGameStartedEvent(PlayerId playerId)
   {
      var game = Game.StartForPlayer(playerId);

      var startEvent = game.DomainEvents.Should()
         .ContainSingle(e => e is GameStartedEvent)
         .Subject.As<GameStartedEvent>();

      var joinedEvent = game.DomainEvents.Should()
         .ContainSingle(e => e is PlayerJoinedEvent)
         .Subject.As<PlayerJoinedEvent>();

      startEvent.StartedBy.Should().Be(playerId);
      startEvent.StartedBy.Should().BeOfType<PlayerId>();
      joinedEvent.PlayerId.Should().Be(playerId);
      joinedEvent.TeamId.Should().Be(Game.TeamId.Team1);
   }
}
