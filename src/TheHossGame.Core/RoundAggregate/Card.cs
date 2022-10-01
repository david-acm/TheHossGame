// 🃏 The HossGame 🃏
// <copyright file="Card.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

public class Card
{
   public Card(CardSuit suit, CardRank rank)
   {
      this.Suit = suit;
      this.Rank = rank;
   }

   public CardSuit Suit { get; }

   public CardRank Rank { get; }

   public override string ToString() => $"{this.Rank.Value} \t {this.Suit.Value}";
}
