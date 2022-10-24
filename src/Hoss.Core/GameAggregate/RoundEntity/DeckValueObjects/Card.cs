// 🃏 The HossGame 🃏
// <copyright file="Card.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Hoss.SharedKernel;

#endregion

public record ACard(CardSuit Suit, CardRank Rank) : Card
{
   public override CardSuit Suit { get; } = Suit;

   public override CardRank Rank { get; } = Rank;

   public override string ToString()
   {
      return $"{this.Rank.Value} \t {this.Suit.Value}";
   }
}

public abstract record Card : ValueObject
{
   public abstract CardSuit Suit { get; }

   public abstract CardRank Rank { get; }
}
