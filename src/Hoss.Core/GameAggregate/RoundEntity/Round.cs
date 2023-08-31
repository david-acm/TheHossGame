﻿// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;

#endregion

public abstract class Round : EntityBase<RoundId>
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

    protected Round(RoundId id, Action<DomainEventBase> when)
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

    internal virtual Deal DealForPlayer(PlayerId playerId) => new(new NoPlayerId());

    internal virtual IEnumerable<Card> CardsForPlayer(PlayerId playerId) => new List<Card>();

    internal abstract void RequestHoss(PlayerId playerId, Card card);

    internal abstract void GiveHossCard(PlayerId playerId, Card card);
}