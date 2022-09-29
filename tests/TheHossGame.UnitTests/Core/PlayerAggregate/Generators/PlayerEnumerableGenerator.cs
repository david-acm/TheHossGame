// 🃏 The HossGame 🃏
// <copyright file="PlayerEnumerableGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture.Kernel;

public class PlayerEnumerableGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(IEnumerable<TheHossGame.Core.PlayerAggregate.Player>).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable(context);
   }

   private static object GeneratePlayerEnumerable(ISpecimenContext context)
   {
      var playerList = new List<TheHossGame.Core.PlayerAggregate.Player>();

      playerList.Add(GeneratePLayer(context));
      playerList.Add(GeneratePLayer(context));
      playerList.Add(GeneratePLayer(context));
      playerList.Add(GeneratePLayer(context));

      return playerList;
   }

   private static TheHossGame.Core.PlayerAggregate.Player GeneratePLayer(ISpecimenContext context)
   {
      var generator = new PlayerGenerator();
      var request = typeof(TheHossGame.Core.PlayerAggregate.Player);
      return (TheHossGame.Core.PlayerAggregate.Player)generator.Create(request, context);
   }
}
