// 🃏 The HossGame 🃏
// <copyright file="PlayerDeal.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region



#endregion

public record Deal(PlayerId PlayerId) : Play(PlayerId)
{
    public virtual List<Card> Cards { get; set; } = new();

    internal virtual void ReceiveCard(Card card)
    {
    }

    internal virtual void PlayCard(Card card)
    {
    }

    internal virtual void GiveCard(Card card)
    {
    }
}

public record ADeal(PlayerId PlayerId) : Deal(PlayerId)
{
    // public List<Card> Cards { get; set; } = new();

    internal override void ReceiveCard(Card card)
    {
        Cards.Add(card);
        base.ReceiveCard(card);
    }

    internal override void GiveCard(Card card)
    {
        Cards.Remove(card);
    }

    internal override void PlayCard(Card card)
    {
        Cards.Remove(card);
        base.PlayCard(card);
    }
}