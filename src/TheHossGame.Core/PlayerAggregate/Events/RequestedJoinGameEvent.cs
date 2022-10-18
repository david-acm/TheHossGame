// 🃏 The HossGame 🃏
// <copyright file="RequestedJoinGameEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate.Events;

using TheHossGame.Core.GameAggregate;
using TheHossGame.SharedKernel;

public record RequestedJoinGameEvent(GameId GameId)
   : DomainEventBase(GameId);