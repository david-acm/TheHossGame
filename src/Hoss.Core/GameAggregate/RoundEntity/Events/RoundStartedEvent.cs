// 🃏 The HossGame 🃏
// <copyright file="RoundStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.Events;

#region

using Hoss.Core.GameAggregate.Events;
using Hoss.SharedKernel;

#endregion

public record RoundStartedEvent(GameId GameId, RoundId RoundId, IEnumerable<RoundPlayer> TeamPlayers) : RoundEventBase(GameId, RoundId);
