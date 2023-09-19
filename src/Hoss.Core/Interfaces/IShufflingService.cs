// 🃏 The HossGame 🃏
// <copyright file="IShufflingService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.Interfaces;

#region

using System.Security.Cryptography;
using GameAggregate.RoundEntity.DeckValueObjects;

#endregion

public interface IShufflingService
{
   void Shuffle(IList<ACard> cards);
}

public class ShufflingService : IShufflingService
{
   private readonly IRandomNumberProvider provider;

   public ShufflingService(IRandomNumberProvider provider)
   {
      this.provider = provider;
   }

   #region IShufflingService Members

   public void Shuffle(IList<ACard> cards)
   {
      var swap = cards.Count;

      while (swap > 1)
      {
         swap--;
         var roll = provider.NextInt(swap + 1);
         (cards[swap], cards[roll]) = (cards[roll], cards[swap]);
      }
   }

   #endregion
}

public interface IRandomNumberProvider
{
   int NextInt(int maxValue);
}

public class RandomNumberProvider : IRandomNumberProvider
{
   #region IRandomNumberProvider Members

   public int NextInt(int maxValue)
   {
      return RandomNumberGenerator.GetInt32(maxValue);
   }

   #endregion
}
