// 🃏 The HossGame 🃏
// <copyright file="PlayerCardsDealtEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.Events;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.SharedKernel;

public record PlayerCardsDealtEvent(
   GameId GameId,
   RoundId RoundId,
   PlayerDeal playerCards)
   : DomainEventBase(GameId)
{

}
