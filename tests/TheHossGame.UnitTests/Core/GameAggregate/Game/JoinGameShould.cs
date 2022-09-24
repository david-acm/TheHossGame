// 🃏 The HossGame 🃏
// <copyright file="JoinGameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate;
using Xunit;

public class JoinGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaisePlayerJoinedEvent(Player player, Game game)
   {
      game.JoinPlayer(player.Id, Game.TeamId.Team1);

      game.DomainEvents
         .Where(e => e is PlayerJoinedEvent)
         .Should().HaveCount(2);
   }

   [Theory]
   [AutoPlayerData]
   public void ThrowInvalidEntityStateWhenTeamsAreComplete(Player player, Game game)
   {
      game.JoinPlayer(player.Id, Game.TeamId.Team1);
      game.JoinPlayer(player.Id, Game.TeamId.Team1);
      var joinPlayerAction = () => game.JoinPlayer(player.Id, Game.TeamId.Team1);

      joinPlayerAction
         .Should().Throw<InvalidEntityStateException>();
   }
}
