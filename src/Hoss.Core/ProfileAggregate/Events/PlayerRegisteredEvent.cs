// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisteredEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.ProfileAggregate.Events;

#region

#endregion

public record PlayerRegisteredEvent
    (ProfileId PlayerId, ProfileEmail Email, PlayerName PlayerName) : DomainEventBase;