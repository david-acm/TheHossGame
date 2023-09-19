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
    public virtual IReadOnlyList<Card> Cards => new List<Card>();

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
    private readonly List<Card> cards = new();

    public override IReadOnlyList<Card> Cards => cards.AsReadOnly();

    internal override void ReceiveCard(Card card)
    {
        cards.Add(card);
        base.ReceiveCard(card);
    }

    internal override void GiveCard(Card card)
    {
        cards.Remove(card);
    }

    internal override void PlayCard(Card card)
    {
        cards.Remove(card);
        base.PlayCard(card);
    }
}