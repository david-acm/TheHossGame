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
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
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
      game.JoinPlayerToTeam(playerId, Team1);

      game.Events
         .Where(e => e is PlayerAlreadyInGame)
         .Should().HaveCount(1);
   }

   [Theory]
   [AutoPlayerData]
   public void RaiseTeamsFormedEvent(IEnumerable<APlayer> players, AGame game)
   {
      var playerList = players.ToList();
      game.JoinPlayerToTeam(playerList[0].Id, Team1);
      game.JoinPlayerToTeam(playerList[1].Id, Team2);
      game.JoinPlayerToTeam(playerList[2].Id, Team2);

      game.Events
         .Where(e => e is TeamsFormedEvent)
         .Should().HaveCount(1);
   }

   [Theory]
   [AutoPlayerData]
   public void RaisePlayerJoinedEvent(APlayer player, AGame game)
   {
      game.JoinPlayerToTeam(player.Id, Team1);

      game.Events
         .Where(e => e is PlayerJoinedEvent)
         .Should().HaveCount(2);
   }

   [Theory]
   [AutoPlayerData]
   public void ThrowInvalidEntityStateWhenTeamsAreComplete(IEnumerable<PlayerId> playerIds, AGame game)
   {
      var playerIdList = playerIds.ToList();
      game.JoinPlayerToTeam(playerIdList[0], Team1);
      var joinPlayerAction = () => game.JoinPlayerToTeam(playerIdList[1], Team1);

      joinPlayerAction
         .Should().Throw<InvalidEntityStateException>();
   }
}
