// 🃏 The HossGame 🃏
// <copyright file="AutoShufflingServiceGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.Interfaces;

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
      shufflingService
         .Setup(s => s.Shuffle(It.IsAny<IList<ACard>>()))
         .Callback(
            (IList<ACard> _) =>
            {
            });
      return shufflingService;
   }
}
