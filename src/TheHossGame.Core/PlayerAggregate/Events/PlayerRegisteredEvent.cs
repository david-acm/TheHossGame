// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisteredEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.SharedKernel;

public record PlayerRegisteredEvent(
   APlayerId PlayerId,
   PlayerName PlayerName)
   : DomainEventBase(PlayerId)
{
}
