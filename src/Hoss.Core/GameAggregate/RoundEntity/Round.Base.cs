// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using DeckValueObjects;
using RoundScoreValueObject;

#endregion

public abstract class RoundBase : EntityBase
{
    #region RoundStage enum

    public enum RoundStage
    {
        None,
        ShufflingCards,
        DealingCards,
        Bidding,
        SelectingTrump,
        PlayingCards,
        Played,
        Hossinng,
    }

    #endregion

    protected RoundBase(RoundId id, Action<DomainEventBase> when)
        : base(id, when)
    {
    }

    internal abstract RoundStage Stage { get; }

    internal abstract IReadOnlyList<ADeal> Deals { get; }

    internal abstract IReadOnlyList<RoundPlayer> RoundPlayers { get; }

    internal abstract IReadOnlyList<Bid> Bids { get; }

    internal abstract IReadOnlyList<CardPlay> CardsPlayed { get; }

    internal abstract PlayerId CurrentPlayerId { get; }

    internal abstract Suit SelectedTrump { get; }
    public RoundScore Score { get; protected set; } = RoundScore.New();

    internal virtual void Bid(PlayerId playerId, BidValue value)
    {
    }

    internal virtual void SelectTrump(PlayerId playerId, Suit suit)
    {
    }

    internal virtual void PlayCard(PlayerId playerId, Card card)
    {
    }

    protected override void EnsureValidState()
    {
    }

    protected override void When(DomainEventBase @event)
    {
    }

    internal virtual Deal DealForPlayer(PlayerId playerId)
    {
        return new Deal(new NoPlayerId());
    }

    internal virtual IEnumerable<Card> CardsForPlayer(PlayerId playerId)
    {
        return new List<Card>();
    }

    internal abstract void RequestHoss(PlayerId playerId, Card card);

    internal abstract void GiveHossCard(PlayerId playerId, Card card);
}