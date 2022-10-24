// 🃏 The HossGame 🃏
// <copyright file="AllCardsDealtEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.Events;

#region

using Hoss.Core.GameAggregate.Events;
using Hoss.SharedKernel;

#endregion

public record AllCardsDealtEvent(GameId GameId, RoundId RoundId) : RoundEventBase(GameId, RoundId);
