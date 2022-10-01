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
   private readonly Stack<Card> cards = new ();

   internal ADeck(IShufflingService shufflingService)
   {
      var cardList = new List<Card>();
      cardList.AddRange(
         CardSuit.List.SelectMany(
            suit => CardRank.List.Select(
            rank => new Card(suit, rank))));
      shufflingService.Shuffle(cardList);
      this.cards = new Stack<Card>(cardList);
   }

   public static ADeck FromShuffling(IShufflingService shufflingService)
      => new (shufflingService);

   public IReadOnlyList<Card> Cards => this.cards.ToList().AsReadOnly();

   public bool HasCards => this.cards.Any();

   public Card Deal() => this.cards.Pop();
}

public record NoDeck : Deck
{
   public static NoDeck New => new ();
}

public abstract record Deck : ValueObject
{
}
