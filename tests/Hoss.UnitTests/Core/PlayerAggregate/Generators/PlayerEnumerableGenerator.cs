// 🃏 The HossGame 🃏
// <copyright file="PlayerEnumerableGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture.Kernel;
using Hoss.Core.PlayerAggregate;

#endregion

public class PlayerEnumerableGenerator : ISpecimenBuilder
{
   private ISpecimenContext? specimenContext;

   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      this.specimenContext = context;
      return typeof(IEnumerable<Player>).Equals(request) ? this.GeneratePlayerList() : new NoSpecimen();
   }

   #endregion

   private List<Player> GeneratePlayerList()
   {
      var playerList = new List<Player>
      {
         this.GeneratePLayer(),
         this.GeneratePLayer(),
         this.GeneratePLayer(),
         this.GeneratePLayer(),
      };

      return playerList;
   }

   private Player GeneratePLayer()
   {
      return (Player)new PlayerGenerator().Create(typeof(Player), this.specimenContext!);
   }
}
