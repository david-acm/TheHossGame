// 🃏 The HossGame 🃏
// <copyright file="CurrentRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using DeckValueObjects;

#endregion

public record RoundView(RoundBase currentRoundBase) : ValueObject
{
    private readonly RoundBase currentRoundBase = currentRoundBase;

    public IReadOnlyList<ADeal> PlayerDeals => currentRoundBase.Deals;

    internal PlayerId CurrentPlayerId => currentRoundBase.CurrentPlayerId;

    public RoundId Id => currentRoundBase.Id;

    public RoundBase.RoundStage Stage => currentRoundBase.Stage;

    public IReadOnlyList<Bid> Bids => currentRoundBase.Bids;

    public IReadOnlyList<RoundPlayer> RoundPlayers => currentRoundBase.RoundPlayers;

    public Suit TrumpSelected => currentRoundBase.SelectedTrump;

    public IReadOnlyList<CardPlay> TableCenter => currentRoundBase.CardsPlayed;

    public Deal DealForPlayer(PlayerId playerId)
    {
        return currentRoundBase.DealForPlayer(playerId);
    }

    public Deal DealForCurrentPlayer()
    {
        return currentRoundBase.DealForPlayer(CurrentPlayerId);
    }
}