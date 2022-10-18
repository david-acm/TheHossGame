// 🃏 The HossGame 🃏
// <copyright file="IShufflingService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Interfaces;

using System.Security.Cryptography;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;

public interface IShufflingService
{
   void Shuffle(IList<ACard> cards);
}

public abstract class ShufflingService : IShufflingService
{
   private readonly IRandomNumberProvider provider;

   protected ShufflingService(IRandomNumberProvider provider)
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
         var roll = this.provider.NextInt(swap + 1);
         (cards[swap], cards[roll]) =
            (cards[roll], cards[swap]);
      }
   }

   #endregion
}

public interface IRandomNumberProvider
{
   int NextInt(int maxValue);
}

public abstract class RandomNumberProvider : IRandomNumberProvider
{
   #region IRandomNumberProvider Members

   public int NextInt(int maxValue)
   {
      return RandomNumberGenerator.GetInt32(maxValue);
   }

   #endregion
}
