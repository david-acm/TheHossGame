// 🃏 The HossGame 🃏
// <copyright file="AutoOrderedDeckGenerator.cs" company="Reactive">
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

public class AutoOrderedDeckGenerator : ISpecimenBuilder
{
   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(ADeck).Equals(request))
      {
         return new NoSpecimen();
      }

      return GenerateShuffledDeck();
   }

   #endregion

   private static ADeck GenerateShuffledDeck()
   {
      var shufflingService = new Mock<IShufflingService>();
      shufflingService.Setup(s => s.Shuffle(It.IsAny<IList<ACard>>())).Callback
         ((IList<ACard> _) =>
         {
         });
      return ADeck.ShuffleNew(shufflingService.Object);
   }
}
