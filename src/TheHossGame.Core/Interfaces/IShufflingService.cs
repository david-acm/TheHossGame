// 🃏 The HossGame 🃏
// <copyright file="IShufflingService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Interfaces;

using System.Collections.Generic;
using System.Security.Cryptography;
using TheHossGame.Core.RoundAggregate;

public interface IShufflingService
{
   void Shuffle(IList<Card> cards);
}

public class ShufflingService : IShufflingService
{
   private readonly IRandomNumberProvider provider;

   public ShufflingService(IRandomNumberProvider provider)
   {
      this.provider = provider;
   }

   public void Shuffle(IList<Card> cards)
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

public class RandomNumberProvider : IRandomNumberProvider
{
   public int NextInt(int maxValue) => RandomNumberGenerator.GetInt32(maxValue);
}