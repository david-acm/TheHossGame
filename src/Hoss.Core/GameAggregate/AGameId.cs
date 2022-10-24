// 🃏 The HossGame 🃏
// <copyright file="AGameId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

#region

using Hoss.SharedKernel;

#endregion

public abstract record GameId : ValueId;

public record AGameId : GameId;

public record NoGameId : GameId;
