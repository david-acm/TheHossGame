// 🃏 The HossGame 🃏
// <copyright file="RoundPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using static Game;

#endregion

public record RoundPlayer(PlayerId PlayerId, TeamId TeamId) : ValueObject;