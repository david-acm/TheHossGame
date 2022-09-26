// 🃏 The HossGame 🃏
// <copyright file="PlayerReadyShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate;
using Xunit;

public class PlayerReadyShould
{
   [Theory]
   [AutoPlayerData]
   public void RaisePlayerReadyEventWhenPlayerIsMember(
      [Frozen] PlayerId playerId,
      AGame game)
   {
      game.TeamPlayerReady(playerId);

      var readyEvent = game.Events
         .Where(e => e is PlayerReadyEvent)
         .Should().ContainSingle()
         .Subject.As<PlayerReadyEvent>();

      readyEvent.Should().NotBeNull();
      readyEvent.PlayerId.Should().Be(playerId);
      readyEvent.GameId.Should().Be(game.Id);
      game.TeamPlayer(playerId).IsReady
         .Should().BeTrue();
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

      game.TeamPlayer(playerId).IsReady
         .Should().BeFalse();
   }
}
