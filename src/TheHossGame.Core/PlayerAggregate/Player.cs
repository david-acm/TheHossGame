// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class Player : EntityBase, IAggregateRoot
{
   private readonly PlayerName name;
   private readonly PlayerEmail email;

   public Player(PlayerName name, PlayerEmail email)
   {
      Guard.Against.Null(name);
      Guard.Against.Null(email);

      this.email = email;
      this.name = name;
   }
}
