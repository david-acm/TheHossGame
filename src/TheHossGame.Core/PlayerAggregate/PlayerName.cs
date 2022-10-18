// 🃏 The HossGame 🃏
// <copyright file="PlayerName.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using TheHossGame.SharedKernel;

public record PlayerName
{
   public PlayerName(string name)
   {
      Guard.Against.NullOrEmpty(name);
      GuardExtensions.InvalidLength(name, nameof(name), 1, 30);

      this.Name = name;
   }

   public string Name { get; }
}
