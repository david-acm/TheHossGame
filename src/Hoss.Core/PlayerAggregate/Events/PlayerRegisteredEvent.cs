// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisteredEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate.Events;

#region

using Hoss.SharedKernel;

#endregion

public record PlayerRegisteredEvent(PlayerId PlayerId, PlayerName PlayerName) : DomainEventBase(PlayerId);
