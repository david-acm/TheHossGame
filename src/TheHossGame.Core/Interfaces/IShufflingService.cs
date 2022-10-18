// 🃏 The HossGame 🃏
// <copyright file="IShufflingService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Interfaces;

using System.Collections.Generic;
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

   public void Shuffle(IList<ACard> cards)
   {
      int swap = cards.Count;

      while (swap > 1)
      {
         swap--;
         int roll = this.provider.NextInt(swap + 1);
         (cards[swap], cards[roll]) =
         (cards[roll], cards[swap]);
      }
   }
}

public interface IRandomNumberProvider
{
   int NextInt(int maxValue);
}

public abstract class RandomNumberProvider : IRandomNumberProvider
{
   public int NextInt(int maxValue) => RandomNumberGenerator.GetInt32(maxValue);
}