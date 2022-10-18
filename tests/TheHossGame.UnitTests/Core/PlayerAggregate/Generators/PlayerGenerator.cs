// 🃏 The HossGame 🃏
// <copyright file="PlayerGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.Kernel;
using TheHossGame.Core.PlayerAggregate;

public class PlayerGenerator : ISpecimenBuilder
{
   private ISpecimenContext? specimenContext;

   public object Create(object request, ISpecimenContext context)
   {
      this.specimenContext = context;
      if (!typeof(Player).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.RandomPlayerEmail();
   }

   private APlayer RandomPlayerEmail()
   {
      var playerId = this.specimenContext.Create<APlayerId>();
      var playerName = this.specimenContext.Create<PlayerName>();
      return APlayer.FromRegister(playerId, playerName);
   }
}
