// 🃏 The HossGame 🃏
// <copyright file="Deck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using System.Linq;
using TheHossGame.Core.Interfaces;
using TheHossGame.SharedKernel;

public record ADeck : Deck
{
   private readonly Stack<ACard> cards = new ();

   internal ADeck(IShufflingService shufflingService)
   {
      var cardList = new List<ACard>();
      cardList.AddRange(
         CardSuit.List.SelectMany(
            suit => CardRank.List.Select(
            rank => new ACard(suit, rank))));
      shufflingService.Shuffle(cardList);
      this.cards = new Stack<ACard>(cardList);
   }

   public static ADeck ShuffleNew(IShufflingService shufflingService)
      => new (shufflingService);

   public override IReadOnlyList<ACard> Cards => this.cards.ToList().AsReadOnly();

   public override bool HasCards => this.cards.Any();

   public override Card Deal() => this.cards.Pop();
}

public record NoDeck : Deck
{
   public static NoDeck New => new ();

   public override IReadOnlyList<ACard> Cards => new List<ACard>();

   public override bool HasCards => false;

   public override Card Deal()
   {
      return NoCard.New;
   }
}

public abstract record Deck : ValueObject
{
   public abstract IReadOnlyList<ACard> Cards { get; }

   public abstract bool HasCards { get; }

   public abstract Card Deal();
}
