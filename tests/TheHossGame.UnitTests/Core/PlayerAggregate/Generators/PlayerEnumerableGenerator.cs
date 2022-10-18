// 🃏 The HossGame 🃏
// <copyright file="PlayerEnumerableGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture.Kernel;
using System.Collections.Generic;
using TheHossGame.Core.PlayerAggregate;

public class PlayerEnumerableGenerator : ISpecimenBuilder
{
   private ISpecimenContext? specimenContext;

   public object Create(object request, ISpecimenContext context)
   {
      this.specimenContext = context;
      return typeof(IEnumerable<Player>).Equals(request) ? this.GeneratePlayerList() : new NoSpecimen();
   }

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
      => (Player)new PlayerGenerator().Create(typeof(Player), this.specimenContext!);
}
