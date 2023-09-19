// 🃏 The HossGame 🃏
// <copyright file="CardPlayedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

using DeckValueObjects;
using RoundScoreValueObject;

public static class RoundEvents
{
    #region Nested type: AllCardsDealtEvent

    public record AllCardsDealtEvent(GameId GameId, RoundId RoundId) : RoundEventBase(GameId, RoundId);

    #endregion

    #region Nested type: BidCompleteEvent

    public record BidCompleteEvent(GameId GameId, RoundId RoundId, Bid WinningBid) : RoundEventBase(GameId, RoundId);

    #endregion

    #region Nested type: BidEvent

    public record BidEvent(GameId GameId, RoundId RoundId, Bid Bid) : RoundEventBase(GameId, RoundId);

    #endregion

    #region Nested type: CardPlayedEvent

    public record CardPlayedEvent(GameId GameId, RoundId RoundId, PlayerId PlayerId, Card Card) : RoundEventBase(GameId,
        RoundId);

    #endregion

    #region Nested type: HossRequestedEvent

    public record HossRequestedEvent(GameId GameId, RoundId RoundId, HossRequest HossRequest) : RoundEventBase(GameId,
        RoundId);

    #endregion

    #region Nested type: PartnerHossCardGivenEvent

    public record PartnerHossCardGivenEvent
        (GameId GameId, RoundId RoundId, HossPartnerCard HossPartnerCard) : RoundEventBase(GameId, RoundId);

    #endregion

    #region Nested type: PlayerCardsDealtEvent

    public record PlayerCardsDealtEvent(GameId GameId, RoundId RoundId, ADeal Deal) : RoundEventBase(GameId, RoundId);

    #endregion

    #region Nested type: RoundEventBase

    public record RoundEventBase(GameId GameId, RoundId RoundId) : GameEvents.GameEventBase(GameId);

    #endregion

    #region Nested type: RoundPlayedEvent

    public record RoundPlayedEvent(GameId GameId, RoundId RoundId, RoundScore RoundScore) : RoundEventBase(GameId,
        RoundId);

    #endregion

    #region Nested type: RoundStartedEvent

    public record RoundStartedEvent
        (GameId GameId, RoundId RoundId, IEnumerable<RoundPlayer> TeamPlayers) : RoundEventBase(GameId, RoundId);

    #endregion

    #region Nested type: TrickPlayedEvent

    public record TrickPlayedEvent(GameId GameId, RoundId RoundId, CardPlay HandWinner) : RoundEventBase(GameId,
        RoundId);

    #endregion

    #region Nested type: TrumpSelectedEvent

    public record TrumpSelectedEvent(GameId GameId, RoundId RoundId, PlayerId PlayerId, Suit Trump) : RoundEventBase(
        GameId,
        RoundId);

    #endregion
}