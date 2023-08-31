// 🃏 The HossGame 🃏
// <copyright file="NoRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.GameAggregate.RoundEntity.Events;
using Hoss.Core.PlayerAggregate;

#endregion

public sealed class NoRound : Round
{
    public NoRound()
        : base(new RoundId(), _ => { })
    {
        this.Apply(new RoundStartedEvent(new NoGameId(), new RoundId(), new List<RoundPlayer>()));
    }

    internal override RoundStage Stage => RoundStage.None;

    internal override IReadOnlyList<ADeal> Deals => new List<ADeal>();

    internal override IReadOnlyList<RoundPlayer> RoundPlayers => new List<RoundPlayer>();

    internal override IReadOnlyList<Bid> Bids => new List<Bid>();

    /// <inheritdoc />
    internal override IReadOnlyList<CardPlay> CardsPlayed => new List<CardPlay>();

    internal override PlayerId CurrentPlayerId => new NoPlayerId();

    internal override Suit SelectedTrump => Suit.None;

    /// <inheritdoc />
    internal override void RequestHoss(PlayerId playerId, Card card)
    {
    }

    /// <inheritdoc />
    internal override void GiveHossCard(PlayerId id, Card card1)
    {
    }
}