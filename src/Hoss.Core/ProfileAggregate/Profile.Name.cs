// 🃏 The HossGame 🃏
// <copyright file="PlayerName.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate;

#region

using Ardalis.GuardClauses;

#endregion

public record PlayerName : NameBase
{
    public PlayerName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        GuardExtensions.InvalidLength(name, nameof(name), 1, 30);

        this.Name = name;
    }

    public string Name { get; }

    public static PlayerName FromString(string name)
    {
        return new PlayerName(name);
    }
}

public record NameBase;

public record NoName : NameBase;