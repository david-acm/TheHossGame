﻿// 🃏 The HossGame 🃏
// <copyright file="GameStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Doesn't applut to records.")]
public record GameStartedEvent(PlayerId StartedBy)
   : DomainEventBase(StartedBy)
{
}