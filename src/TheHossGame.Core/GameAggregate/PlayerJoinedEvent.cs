// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public record PlayerJoinedEvent(APlayerId PlayerId, TeamId TeamId)
   : DomainEventBase(PlayerId)
{
}
