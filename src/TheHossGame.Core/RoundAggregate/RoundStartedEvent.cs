// 🃏 The HossGame 🃏
// <copyright file="RoundStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record RoundStartedEvent(
   GameId GameId,
   RoundId RoundId,
   IEnumerable<PlayerId> PlayerIds)
   : DomainEventBase(GameId)
{
}

public record PlayerCardsDealtEvent(
   GameId GameId,
   RoundId RoundId,
   PlayerDeal playerCards)
   : DomainEventBase(GameId)
{

}

public record AllCardsDealtEvent(
   GameId GameId,
   RoundId RoundId)
   : DomainEventBase(GameId)
{

}