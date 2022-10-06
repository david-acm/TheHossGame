// 🃏 The HossGame 🃏
// <copyright file="RoundGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.RoundAggregate;

public class RoundGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(Round).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.GeneratePlayerEnumerable();
   }

   private Round GeneratePlayerEnumerable()
   {
      var game = this.context.Create<AGame>();
      var shuffleService = this.context.Create<Mock<IShufflingService>>();
      var teamPlayers = game.FindTeamPlayers().Select(g => new TeamPlayer(g.PlayerId, g.TeamId));
      return Round.StartNew(game.Id, teamPlayers, shuffleService.Object);
   }
}
