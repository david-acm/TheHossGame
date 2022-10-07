// 🃏 The HossGame 🃏
// <copyright file="AutoOrderedDeckGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.RoundAggregate;

public class AutoOrderedDeckGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(ADeck).Equals(request))
      {
         return new NoSpecimen();
      }

      return GenerateShuffledDeck();
   }

   private static ADeck GenerateShuffledDeck()
   {
      var shufflingService = new Mock<IShufflingService>();
      var cards = new List<Card>();
      shufflingService
         .Setup(s => s.Shuffle(It.IsAny<IList<Card>>()))
         .Callback((IList<Card> a) => cards = a.ToList());
      return ADeck.FromShuffling(shufflingService.Object);
   }
}
