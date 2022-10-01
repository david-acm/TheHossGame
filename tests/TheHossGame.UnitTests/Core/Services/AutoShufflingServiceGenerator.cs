// 🃏 The HossGame 🃏
// <copyright file="AutoShufflingServiceGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.RoundAggregate;

public class AutoShufflingServiceGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(Mock<IShufflingService>).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable();
   }

   private static Mock<IShufflingService> GeneratePlayerEnumerable()
   {
      var shufflingService = new Mock<IShufflingService>();
      var cards = new List<Card>();
      shufflingService
         .Setup(s => s.Shuffle(It.IsAny<IList<Card>>()))
         .Callback((IList<Card> a) => cards = a.ToList());
      return shufflingService;
   }
}