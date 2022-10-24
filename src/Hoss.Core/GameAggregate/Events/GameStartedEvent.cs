// 🃏 The HossGame 🃏
// <copyright file="GameStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.Events;

#region

using System.Diagnostics.CodeAnalysis;
using Hoss.Core.PlayerAggregate;

#endregion

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Doesn't apply to records.")]
public record NewGameCreatedEvent(GameId GameId, PlayerId StartedBy) : GameEventBase(GameId);
