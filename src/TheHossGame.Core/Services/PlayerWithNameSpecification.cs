// 🃏 The HossGame 🃏
// <copyright file="PlayerWithNameSpecification.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Services;

using Ardalis.Specification;
using TheHossGame.Core.PlayerAggregate;

public class PlayerWithNameSpec : Specification<APlayer>
{
   private readonly APlayer player;

   public PlayerWithNameSpec(APlayer player)
   {
      this.player = player;
   }
}
