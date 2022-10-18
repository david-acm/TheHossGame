// 🃏 The HossGame 🃏
// <copyright file="AutoOrderedDeckGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.Interfaces;

public class AutoOrderedDeckGenerator : ISpecimenBuilder
{
   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(ADeck).Equals(request))
      {
         return new NoSpecimen();
      }

      return GenerateShuffledDeck();
   }

   private static ADeck GenerateShuffledDeck()
   {
      var shufflingService = new Mock<IShufflingService>();
      shufflingService
         .Setup(s => s.Shuffle(It.IsAny<IList<ACard>>()))
         .Callback((IList<ACard> _) =>
         {
         });
      return ADeck.ShuffleNew(shufflingService.Object);
   }
}
