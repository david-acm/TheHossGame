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

public record ACard(Suit Suit, Rank Rank) : Card
{
   public override Suit Suit { get; } = Suit;

   public override Rank Rank { get; } = Rank;

   public override string ToString()
   {
      return $"{this.Rank.Value} \t {this.Suit.Value}";
   }
}

public abstract record Card : ValueObject
{
   public abstract Suit Suit { get; }

   public abstract Rank Rank { get; }
}
