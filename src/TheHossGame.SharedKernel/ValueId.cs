// 🃏 The HossGame 🃏
// <copyright file="ValueId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.SharedKernel;

/// <summary>
///    A value object with guid identity.
/// </summary>
public abstract record ValueId : ValueObject
{
   private readonly Guid id;

   /// <summary>
   ///    Initializes a new instance of the <see cref="ValueId" /> class.
   /// </summary>
   protected ValueId()
   {
      this.id = Guid.NewGuid();
   }
}
