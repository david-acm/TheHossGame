// 🃏 The HossGame 🃏
// <copyright file="GameEventBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.Events;

#region

using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.SharedKernel;

#endregion

public record GameEventBase(GameId GameId) : DomainEventBase(GameId);

public record RoundEventBase(GameId GameId, RoundId RoundId) : GameEventBase(GameId);
