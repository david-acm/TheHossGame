// 🃏 The HossGame 🃏
// <copyright file="AutoShufflingServiceGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

   #region

using AutoFixture.Kernel;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.Interfaces;
using Moq;

#endregion

public class AutoShufflingServiceGenerator : ISpecimenBuilder
{
   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(Mock<IShufflingService>).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable();
   }

   #endregion

   private static Mock<IShufflingService> GeneratePlayerEnumerable()
   {
      var shufflingService = new Mock<IShufflingService>();
      shufflingService.Setup(s => s.Shuffle(It.IsAny<IList<ACard>>())).Callback
         ((IList<ACard> cards) =>
         {
            var orderedCards = cards.OrderBy(c => c.Rank).ToList();
            var card1 = orderedCards[4];
            var card2 = orderedCards[2];
            orderedCards[4] = card2;
            orderedCards[2] = card1;

            cards.Clear();
            orderedCards.ForEach(cards.Add);
         });
      return shufflingService;
   }
}

public class AutoHossShufflingServiceGenerator : ISpecimenBuilder
{
   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(Mock<IShufflingService>).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable();
   }

   #endregion

   private static Mock<IShufflingService> GeneratePlayerEnumerable()
   {
      var shufflingService = new Mock<IShufflingService>();
      shufflingService.Setup(s => s.Shuffle(It.IsAny<IList<ACard>>())).Callback
         ((IList<ACard> cards) =>
         {
            var ordererdCards = cards.OrderBy(c => c.Rank).ToList();
            cards.Clear();
            ordererdCards.ForEach(c => cards.Add(c));
         });
      return shufflingService;
   }
}
