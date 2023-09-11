// 🃏 The HossGame 🃏
// <copyright file="PlayerId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

public record APlayerId : PlayerId
{
    /// <inheritdoc />
    public override string ToString()
    {
        return this.Id.ToString();
    }
}

public record NoPlayerId : PlayerId;

public abstract record PlayerId : ValueId;