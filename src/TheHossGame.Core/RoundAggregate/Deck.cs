// 🃏 The HossGame 🃏
// <copyright file="Deck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.Interfaces;
using TheHossGame.SharedKernel;

public record ADeck : Deck
{
   private readonly List<Card> cards = new ();

   internal ADeck(IShufflingService shufflingService)
   {
      this.cards.AddRange(
         CardSuit.List.SelectMany(
            suit => CardRank.List.Select(
            rank => new Card(suit, rank))));
      shufflingService.Shuffle(this.cards);
   }

   public static ADeck FromShuffling(IShufflingService shufflingService)
      => new (shufflingService);

   public IReadOnlyList<Card> Cards => this.cards.AsReadOnly();
}

public record NoDeck : Deck
{
   public static NoDeck New => new ();
}

public abstract record Deck : ValueObject
{
}
