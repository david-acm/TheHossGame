// 🃏 The HossGame 🃏
// <copyright file="PlayerDeal.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record PlayerDeal
   : ValueObject
{
   private readonly List<Card> cards = new ();

   public PlayerDeal(PlayerId PlayerId)
   {
      this.PlayerId = PlayerId;
   }

   public PlayerId PlayerId { get; }
   public IReadOnlyList<Card> Cards => this.cards.AsReadOnly();

   public void ReceibeCard(Card card)
   {
      this.cards.Add(card);
   }
}