// 🃏 The HossGame 🃏
// <copyright file="PlayerName.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using TheHossGame.SharedKernel;

public class PlayerName : ValueObject
{
   public PlayerName(string name)
   {
      Guard.Against.NullOrEmpty(name);

      this.Name = name;
   }

   public string Name { get; private set; }
}