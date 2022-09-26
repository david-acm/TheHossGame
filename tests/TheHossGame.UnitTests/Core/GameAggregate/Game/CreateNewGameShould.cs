// 🃏 The HossGame 🃏
// <copyright file="CreateNewGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using System;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate;
using Xunit;
using static TheHossGame.Core.GameAggregate.Game.TeamId;

public class CreateNewGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaiseGameStartedEvent(APlayerId playerId)
   {
      var game = AGame.CreateNewForPlayer(playerId);

      var startEvent = game.Events.Should()
         .ContainSingle(e => e is NewGameCreatedEvent)
         .Subject.As<NewGameCreatedEvent>();

      var joinedEvent = game.Events.Should()
         .ContainSingle(e => e is PlayerJoinedEvent)
         .Subject.As<PlayerJoinedEvent>();

      startEvent.StartedBy.Should().Be(playerId);
      startEvent.StartedBy.Should().BeOfType<APlayerId>();
      joinedEvent.PlayerId.Should().Be(playerId);
      joinedEvent.TeamId.Should().Be(Team1);
   }
}
