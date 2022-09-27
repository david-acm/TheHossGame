// 🃏 The HossGame 🃏
// <copyright file="ValueId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using TheHossGame.SharedKernel;

public abstract record ValueId : ValueObject
{
   protected ValueId()
   {
      this.Id = Guid.NewGuid();
   }

   public Guid Id { get; }
}