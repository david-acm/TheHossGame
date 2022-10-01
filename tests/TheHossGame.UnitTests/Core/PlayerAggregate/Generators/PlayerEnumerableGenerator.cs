// 🃏 The HossGame 🃏
// <copyright file="PlayerEnumerableGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using System.Collections.Generic;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;

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
      var gameId = this.context.Create<AGameId>();
      var roundPlayers = this.context.Create<IEnumerable<Player>>();
      var shuffleService = this.context.Create<Mock<IShufflingService>>();
      return Round.StartNew(gameId, roundPlayers.Select(p => p.Id), shuffleService.Object);
   }
}
