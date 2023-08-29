// 🃏 The HossGame 🃏
// <copyright file="CardPlayedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;

public record CardPlayedEvent(GameId GameId, RoundId RoundId, PlayerId PlayerId, Card Card) : RoundEventBase(GameId,
    RoundId);

public record HandPlayedEvent(GameId GameId, RoundId RoundId, CardPlay HandWinner) : RoundEventBase(GameId, RoundId);