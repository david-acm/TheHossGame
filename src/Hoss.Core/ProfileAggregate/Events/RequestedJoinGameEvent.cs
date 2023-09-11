// 🃏 The HossGame 🃏
// <copyright file="RequestedJoinGameEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate.Events;

#region



#endregion

public record RequestedJoinGameEvent(ProfileId Id, ValueId GameId) : ProfileEventBase(Id);