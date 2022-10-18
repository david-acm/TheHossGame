// ---
// 🃏 The HossGame 🃏
// <copyright file="DefaultIntId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

// This can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)

/// <summary>
/// A default value object that uses int for entity id.
/// </summary>
public record DefaultIntId : ValueObject
{
   /// <summary>
   /// Initializes a new instance of the <see cref="DefaultIntId"/> class.
   /// </summary>
   /// <param name="id">The id.</param>
   public DefaultIntId(int id)
   {
      this.Value = id;
   }

   /// <summary>
   /// Gets the id.
   /// </summary>
   public int Value { get; }

   /// <summary>
   /// To int implicit conversion.
   /// </summary>
   /// <param name="value">The entity Id.</param>
   public static implicit operator int(DefaultIntId value)
   {
      return value.Value;
   }

   /// <summary>
   /// To int implicit conversion.
   /// </summary>
   /// <returns>The entity Id.</returns>
   public int ToInt32() => this;
}
