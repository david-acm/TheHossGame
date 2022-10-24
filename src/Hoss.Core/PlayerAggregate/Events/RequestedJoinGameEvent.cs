// 🃏 The HossGame 🃏
// <copyright file="RequestedJoinGameEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate.Events;

#region

using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;

#endregion

public record RequestedJoinGameEvent(PlayerId Id, GameId GameId) : PlayerEventBase(Id);
