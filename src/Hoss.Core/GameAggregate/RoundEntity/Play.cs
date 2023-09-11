// 🃏 The HossGame 🃏
// <copyright file="Play.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

public abstract record Play(PlayerId PlayerId) : ValueObject
{
    public PlayerId PlayerId { get; } = PlayerId;
}