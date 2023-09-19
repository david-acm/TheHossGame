// 🃏 The HossGame 🃏
// <copyright file="PlayerId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

public record APlayerId : PlayerId
{
    public APlayerId(Guid playerId) 
        : base (playerId)
    {
    }
}

public record NoPlayerId() : PlayerId(Guid.Empty);

public record PlayerId(Guid Id) : ValueId
{
    public static implicit operator PlayerId(Guid s) => new(s);
    public static implicit operator Guid(PlayerId s) => s.Id;
}