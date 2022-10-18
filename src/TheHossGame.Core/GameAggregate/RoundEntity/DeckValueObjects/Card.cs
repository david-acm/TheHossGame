// 🃏 The HossGame 🃏
// <copyright file="Card.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;

using TheHossGame.SharedKernel;

public record ACard : Card
{
   public ACard(CardSuit suit, CardRank rank)
   {
      this.Suit = suit;
      this.Rank = rank;
   }

   public override CardSuit Suit { get; }

   public override CardRank Rank { get; }

   public override string ToString()
   {
      return $"{this.Rank.Value} \t {this.Suit.Value}";
   }
}

public abstract record Card : ValueObject
{
   public static NoCard New => new ();

   public abstract CardSuit Suit { get; }

   public abstract CardRank Rank { get; }
}

public record NoCard : Card
{
   public override CardSuit Suit => CardSuit.None;

   public override CardRank Rank => CardRank.None;
}
