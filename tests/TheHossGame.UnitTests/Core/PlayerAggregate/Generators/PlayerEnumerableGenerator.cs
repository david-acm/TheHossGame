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
   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(IEnumerable<Player>).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable(context);
   }

   private static object GeneratePlayerEnumerable(ISpecimenContext context)
   {
      var playerList = new List<Player>
      {
         GeneratePLayer(context),
         GeneratePLayer(context),
         GeneratePLayer(context),
         GeneratePLayer(context),
      };

      return playerList;
   }

   private static Player GeneratePLayer(ISpecimenContext context)
   {
      var generator = new PlayerGenerator();
      var request = typeof(Player);
      return (Player)generator.Create(request, context);
   }
}
