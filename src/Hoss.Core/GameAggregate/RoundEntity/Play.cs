// 🃏 The HossGame 🃏
// <copyright file="Play.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;

public abstract record Play(PlayerId PlayerId) : ValueObject
{
   public PlayerId PlayerId { get; } = PlayerId;
}
