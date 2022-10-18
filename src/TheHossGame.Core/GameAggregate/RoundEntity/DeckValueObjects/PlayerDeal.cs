// 🃏 The HossGame 🃏
// <copyright file="PlayerDeal.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record PlayerDeal(PlayerId PlayerId)
   : ValueObject
{
   private readonly List<Card> cards = new ();

   private PlayerId PlayerId { get; } = PlayerId;
   public IReadOnlyList<Card> Cards => this.cards.AsReadOnly();

   public void ReceiveCard(Card card)
   {
      this.cards.Add(card);
   }
}