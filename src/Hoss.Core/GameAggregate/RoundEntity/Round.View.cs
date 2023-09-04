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

public record RoundView(RoundBase currentRoundBase) : ValueObject
{
    private readonly RoundBase currentRoundBase = currentRoundBase;

    public IReadOnlyList<ADeal> PlayerDeals => this.currentRoundBase.Deals;

    internal PlayerId CurrentPlayerId => this.currentRoundBase.CurrentPlayerId;

    public RoundId Id => this.currentRoundBase.Id;

    public RoundBase.RoundStage Stage => this.currentRoundBase.Stage;

    public IReadOnlyList<Bid> Bids => this.currentRoundBase.Bids;

    public IReadOnlyList<RoundPlayer> RoundPlayers => this.currentRoundBase.RoundPlayers;

    public Suit TrumpSelected => this.currentRoundBase.SelectedTrump;

    public IReadOnlyList<CardPlay> TableCenter => this.currentRoundBase.CardsPlayed;

    public Deal DealForPlayer(PlayerId playerId)
    {
        return this.currentRoundBase.DealForPlayer(playerId);
    }

    public Deal DealForCurrentPlayer()
    {
        return this.currentRoundBase.DealForPlayer(this.CurrentPlayerId);
    }
}