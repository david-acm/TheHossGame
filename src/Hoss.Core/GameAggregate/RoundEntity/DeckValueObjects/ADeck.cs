// 🃏 The HossGame 🃏
// <copyright file="ADeck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Hoss.Core.Interfaces;

#endregion

public sealed record ADeck : Deck
{
   private readonly Stack<ACard> cards = new ();

   private ADeck(IShufflingService shufflingService)
   {
      var cardList = new List<ACard>();
      cardList.AddRange(CardSuit.List.SelectMany(suit => CardRank.List.Select(rank => new ACard(suit, rank))));
      shufflingService.Shuffle(cardList);
      this.cards = new Stack<ACard>(cardList);
   }

   public IReadOnlyList<ACard> Cards => this.cards.ToList().AsReadOnly();

   public override bool HasCards => this.cards.Any();

   public static ADeck ShuffleNew(IShufflingService shufflingService)
   {
      return new ADeck(shufflingService);
   }

   public override Card Deal()
   {
      return this.cards.Pop();
   }
}
