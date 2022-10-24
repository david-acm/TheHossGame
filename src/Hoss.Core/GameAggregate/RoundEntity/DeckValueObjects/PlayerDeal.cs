// 🃏 The HossGame 🃏
// <copyright file="PlayerDeal.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;

#endregion

public record PlayerDeal(PlayerId PlayerId) : ValueObject
{
   private readonly List<Card> cards = new ();

   public IReadOnlyList<Card> Cards => this.cards.AsReadOnly();

   public void ReceiveCard(Card card)
   {
      this.cards.Add(card);
   }
}
