// 🃏 The HossGame 🃏
// <copyright file="BidFinishedGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.Kernel;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.Interfaces;
using Hoss.Core.PlayerAggregate;
using Moq;

#endregion

public class BidFinishedGameGenerator : ISpecimenBuilder
{
   private ISpecimenContext? specimenContext;

   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      this.specimenContext = context;
      if (!typeof(AGame).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.GenerateBidFinishedGame();
   }

   #endregion

   private AGame GenerateBidFinishedGame()
   {
      var shufflingService = this.specimenContext!.Create<Mock<IShufflingService>>();
      var players = this.specimenContext.Create<IEnumerable<Player>>().ToList();

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
