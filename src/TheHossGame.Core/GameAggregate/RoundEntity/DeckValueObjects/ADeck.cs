// 🃏 The HossGame 🃏
// <copyright file="ADeck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;

using System.Linq;
using TheHossGame.Core.Interfaces;

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
