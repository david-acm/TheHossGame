// 🃏 The HossGame 🃏
// <copyright file="BidEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.Events;

#region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;

#endregion

public record BidEvent(GameId GameId, RoundId RoundId, Bid Bid) : RoundEventBase(GameId, RoundId);

public record HossRequestedEvent(GameId GameId, RoundId RoundId, HossRequest HossRequest) : RoundEventBase(GameId,
    RoundId);

public record PartnerHossCardGivenEvent
    (GameId GameId, RoundId RoundId, HossPartnerCard HossPartnerCard) : RoundEventBase(GameId, RoundId);

public record BidCompleteEvent(GameId GameId, RoundId RoundId, Bid WinningBid) : RoundEventBase(GameId, RoundId);

public record TrumpSelectedEvent(GameId GameId, RoundId RoundId, PlayerId PlayerId, Suit Trump) : RoundEventBase(GameId,
    RoundId);