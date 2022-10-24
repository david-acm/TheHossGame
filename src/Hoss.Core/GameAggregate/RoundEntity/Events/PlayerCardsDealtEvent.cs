// 🃏 The HossGame 🃏
// <copyright file="PlayerCardsDealtEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.Events;

   #region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#endregion

public record PlayerCardsDealtEvent(GameId GameId, RoundId RoundId, ADeal Cards) : RoundEventBase(GameId, RoundId);
