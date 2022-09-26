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
using static TheHossGame.Core.GameAggregate.Game;

public class JoinGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaisePlayerJoinedEvent(APlayer player, AGame game)
   {
      game.JoinPlayer(player.Id, AGame.TeamId.Team1);

      game.DomainEvents
         .Where(e => e is PlayerJoinedEvent)
         .Should().HaveCount(2);
   }

   [Theory]
   [AutoPlayerData]
   public void ThrowInvalidEntityStateWhenTeamsAreComplete(APlayer player, AGame game)
   {
      game.JoinPlayer(player.Id, TeamId.Team1);
      game.JoinPlayer(player.Id, TeamId.Team1);
      var joinPlayerAction = () => game.JoinPlayer(player.Id, AGame.TeamId.Team1);

      joinPlayerAction
         .Should().Throw<InvalidEntityStateException>();
   }
}
