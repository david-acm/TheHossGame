// 🃏 The HossGame 🃏
// <copyright file="NoRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using DeckValueObjects;
using static RoundEvents;

#endregion

public sealed class NoRound : RoundBase
{
    public NoRound()
        : base(new RoundId(), _ => { })
    {
        Apply(new RoundStartedEvent(new NoGameId(), new RoundId(), new List<RoundPlayer>()));
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