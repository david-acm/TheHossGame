// 🃏 The HossGame 🃏
// <copyright file="ReadyGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.Interfaces;
using Player = TheHossGame.Core.PlayerAggregate.Player;

public class ReadyGameGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(AGame).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.GenerateReadyGame();
   }

   private AGame GenerateReadyGame()
   {
      var shufflingService = this.context!.Create<Mock<IShufflingService>>();
      var players = this.context.Create<IEnumerable<Player>>().ToList();

      var readyGame = AGame.CreateForPlayer(players.First().Id, shufflingService.Object);

      readyGame.JoinPlayerToTeam(players[1].Id, Game.TeamId.Team1);
      readyGame.JoinPlayerToTeam(players[2].Id, Game.TeamId.Team2);
      readyGame.JoinPlayerToTeam(players[3].Id, Game.TeamId.Team2);

      players.ForEach(player => readyGame.TeamPlayerReady(player.Id));

      return readyGame;
   }
}
