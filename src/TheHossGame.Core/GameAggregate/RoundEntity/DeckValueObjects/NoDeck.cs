// 🃏 The HossGame 🃏
// <copyright file="NoDeck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;

public record NoDeck : Deck
{
   public static NoDeck New => new ();

   public virtual IReadOnlyList<ACard> Cards => new List<ACard>();

   public override bool HasCards => false;

   public override Card Deal()
   {
      return Card.New;
   }
}
