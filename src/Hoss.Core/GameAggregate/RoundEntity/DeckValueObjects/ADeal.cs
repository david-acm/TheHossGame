﻿// 🃏 The HossGame 🃏
// <copyright file="PlayerDeal.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

   #region

using Hoss.Core.PlayerAggregate;

#endregion

public record Deal(PlayerId PlayerId) : Play(PlayerId)
{
   public virtual IReadOnlyList<Card> Cards => new List<Card>();

   internal virtual void ReceiveCard(Card card)
   {
   }

   internal virtual void PlayCard(Card card)
   {
   }
}

public record ADeal(PlayerId PlayerId) : Deal(PlayerId)
{
   private readonly List<Card> cards = new ();

   public override IReadOnlyList<Card> Cards => this.cards.AsReadOnly();

   internal override void ReceiveCard(Card card)
   {
      this.cards.Add(card);
      base.ReceiveCard(card);
   }

   internal override void PlayCard(Card card)
   {
      this.cards.Remove(card);
      base.PlayCard(card);
   }
}