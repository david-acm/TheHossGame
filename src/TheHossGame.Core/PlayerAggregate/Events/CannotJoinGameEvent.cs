// 🃏 The HossGame 🃏
// <copyright file="CannotJoinGameEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate.Events;

using TheHossGame.SharedKernel;

public record CannotJoinGameEvent(PlayerId PlayerId, string Reason)
   : DomainEventBase(PlayerId);
