// 🃏 The HossGame 🃏
// <copyright file="PlayerId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using TheHossGame.SharedKernel;
public record APlayerId : PlayerId
{
}

public record NoPlayerId : PlayerId
{
}

public abstract record PlayerId : ValueId
{
}
