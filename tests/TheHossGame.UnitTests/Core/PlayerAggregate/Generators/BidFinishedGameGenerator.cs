// 🃏 The HossGame 🃏
// <copyright file="BidFinishedGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.Interfaces;
using Player = TheHossGame.Core.PlayerAggregate.Player;

public class BidFinishedGameGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(AGame).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.GenerateBidFinishedGame();
   }

   private AGame GenerateBidFinishedGame()
   {
      var shufflingService = this.context!.Create<Mock<IShufflingService>>();
      var players = this.context.Create<IEnumerable<Player>>().ToList();

      var game = AGame.CreateForPlayer(players.First().Id, shufflingService.Object);

      game.JoinPlayerToTeam(players[1].Id, Game.TeamId.Team1);
      game.JoinPlayerToTeam(players[2].Id, Game.TeamId.Team2);
      game.JoinPlayerToTeam(players[3].Id, Game.TeamId.Team2);

      players.ForEach(player => game.TeamPlayerReady(player.Id));

      game.Bid(game.CurrentPlayerId, BidValue.One);
      game.Bid(game.CurrentPlayerId, BidValue.Two);
      game.Bid(game.CurrentPlayerId, BidValue.Three);
      game.Bid(game.CurrentPlayerId, BidValue.Pass);

      return game;
   }
}