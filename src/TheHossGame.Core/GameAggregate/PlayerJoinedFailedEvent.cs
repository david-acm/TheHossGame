// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedFailedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record PlayerJoinedFailedEvent(APlayerId playerId)
   : DomainEventBase(playerId)
{
}