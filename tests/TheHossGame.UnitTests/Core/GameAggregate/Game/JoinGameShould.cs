// 🃏 The HossGame 🃏
// <copyright file="JoinGameShould.cs" company="Reactive">
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
using static TheHossGame.Core.GameAggregate.Game.TeamId;

public class JoinGameShould
{
   [Theory]
   [AutoPlayerData]
   public void RaisePlayerAlreadyInGame(
      [Frozen] PlayerId playerId,
      AGame game)
   {
      game.JoinPlayer(playerId, Team1);

      game.Events
         .Where(e => e is PlayerAlreadyInGame)
         .Should().HaveCount(1);
   }

   [Theory]
   [AutoPlayerData]
   public void RaiseTeamsFormedEvent(IEnumerable<APlayer> players, AGame game)
   {
      var playerList = players.ToList();
      game.JoinPlayer(playerList[0].Id, Team1);
      game.JoinPlayer(playerList[1].Id, Team2);
      game.JoinPlayer(playerList[2].Id, Team2);

      game.Events
         .Where(e => e is TeamsFormedEvent)
         .Should().HaveCount(1);
   }

   [Theory]
   [AutoPlayerData]
   public void RaisePlayerJoinedEvent(APlayer player, AGame game)
   {
      game.JoinPlayer(player.Id, Team1);

      game.Events
         .Where(e => e is PlayerJoinedEvent)
         .Should().HaveCount(2);
   }

   [Theory]
   [AutoPlayerData]
   public void ThrowInvalidEntityStateWhenTeamsAreComplete(IEnumerable<PlayerId> playerIds, AGame game)
   {
      var playerIdList = playerIds.ToList();
      game.JoinPlayer(playerIdList[0], Team1);
      var joinPlayerAction = () => game.JoinPlayer(playerIdList[1], Team1);

      joinPlayerAction
         .Should().Throw<InvalidEntityStateException>();
   }
}
