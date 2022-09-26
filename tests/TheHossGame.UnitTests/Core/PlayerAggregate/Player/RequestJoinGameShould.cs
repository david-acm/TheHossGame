// 🃏 The HossGame 🃏
// <copyright file="RequestJoinGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.PlayerAggregate.Events;
using Xunit;

public class RequestJoinGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaiseRequestJoinGameEvent(
      APlayer player, GameId gameId)
   {
      player.RequestJoinGame(gameId);

      player.DomainEvents
         .Should().ContainSingle(e => e is RequestedJoinGameEvent)
         .Which.Should().BeOfType<RequestedJoinGameEvent>()
         .Which.GameId.Should().Be(gameId);
   }

   [Theory]
   [AutoPlayerData]
   public void RaiseCannotJoinGameEventWhenPlayerAlreadyInAGame(
      APlayer player, GameId gameId, GameId anotherGameId)
   {
      player.RequestJoinGame(gameId);
      player.RequestJoinGame(anotherGameId);

      player.DomainEvents
         .Should().ContainSingle(e => e is CannotJoinGameEvent)
         .Which.Should().BeOfType<CannotJoinGameEvent>()
         .Which.Reason.Should().Be("APlayer already in a game");
   }

   [Theory]
   [AutoPlayerData]
   public void ThrowInvalidEntityStateException(APlayer player, NoGameId noGameId)
   {
      var requestAction = () => player.RequestJoinGame(noGameId);

      requestAction.Should().Throw<InvalidEntityStateException>();
   }
}
