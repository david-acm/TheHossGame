// 🃏 The HossGame 🃏
// <copyright file="BidEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.Events;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.SharedKernel;

public record BidEvent(GameId GameId, RoundId RoundId, Bid Bid)
   : DomainEventBase(GameId)
{
}