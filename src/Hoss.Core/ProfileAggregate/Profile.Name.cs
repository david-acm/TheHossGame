// 🃏 The HossGame 🃏
// <copyright file="PlayerName.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Ardalis.GuardClauses;

namespace Hoss.Core.ProfileAggregate;

#region



#endregion

public record PlayerName : NameBase
{
    public PlayerName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        GuardExtensions.InvalidLength(name, nameof(name), 1, 30);

        Name = name;
    }

    public string Name { get; }

    public static PlayerName FromString(string name)
    {
        return new PlayerName(name);
    }
}

public record NameBase;

public record NoName : NameBase;