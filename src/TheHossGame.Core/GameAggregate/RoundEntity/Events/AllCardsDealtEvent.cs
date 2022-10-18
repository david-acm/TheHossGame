// 🃏 The HossGame 🃏
// <copyright file="AllCardsDealtEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.Events;

using TheHossGame.Core.GameAggregate;
using TheHossGame.SharedKernel;

public record AllCardsDealtEvent(
   GameId GameId,
   RoundId RoundId)
   : DomainEventBase(GameId);
