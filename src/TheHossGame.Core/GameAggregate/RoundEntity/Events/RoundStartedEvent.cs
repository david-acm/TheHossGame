// 🃏 The HossGame 🃏
// <copyright file="RoundStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.Events;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.SharedKernel;

public record RoundStartedEvent(
   GameId GameId,
   RoundId RoundId,
   IEnumerable<RoundPlayer> TeamPlayers)
   : DomainEventBase(GameId);
