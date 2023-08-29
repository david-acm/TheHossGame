// 🃏 The HossGame 🃏
// <copyright file="CurrentRound.cs" company="Reactive">
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

public record RoundState(Round currentRound) : ValueObject
{
    private readonly Round currentRound = currentRound;

    public IReadOnlyList<ADeal> PlayerDeals => this.currentRound.Deals;

    internal PlayerId CurrentPlayerId => this.currentRound.CurrentPlayerId;

    public RoundId Id => this.currentRound.Id;

    public Round.RoundState State => this.currentRound.State;

    public IReadOnlyList<Bid> Bids => this.currentRound.Bids;

    public IReadOnlyList<RoundPlayer> RoundPlayers => this.currentRound.RoundPlayers;

    public Suit TrumpSelected => this.currentRound.SelectedTrump;

    public IReadOnlyList<CardPlay> TableCenter => this.currentRound.CardsPlayed;

    public Deal DealForPlayer(PlayerId playerId) => this.currentRound.DealForPlayer(playerId);

    public Deal DealForCurrentPlayer()
    {
        return this.currentRound.DealForPlayer(this.CurrentPlayerId);
    }
}