// 🃏 The HossGame 🃏
// <copyright file="BidEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.Events;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record BidEvent(GameId GameId, RoundId RoundId, Bid Bid)
   : DomainEventBase(GameId)
{
}

public record BidCompleteEvent(GameId GameId, RoundId RoundId, Bid WinningBid)
   : DomainEventBase(GameId)
{
}

public record TrumpSelectedEvent(GameId GameId, RoundId RoundId, PlayerId playerId, CardSuit Trump)
   : DomainEventBase(GameId)
{
}