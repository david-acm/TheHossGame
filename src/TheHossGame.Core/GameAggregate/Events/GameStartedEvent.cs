// 🃏 The HossGame 🃏
// <copyright file="GameStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.Events;

using System.Diagnostics.CodeAnalysis;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

[SuppressMessage(
   "StyleCop.CSharp.NamingRules",
   "SA1313:Parameter names should begin with lower-case letter",
   Justification = "Doesn't apply to records.")]
public record NewGameCreatedEvent(GameId GameId, PlayerId StartedBy)
   : DomainEventBase(StartedBy);
