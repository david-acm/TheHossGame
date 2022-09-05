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
public class DefaultIntId : ValueObject
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
   /// Initializes a new instance of the <see cref="DefaultIntId"/> class.
   /// </summary>
   public DefaultIntId()
   {
   }

   /// <summary>
   /// Gets or sets the id.
   /// </summary>
   public int Value { get; set; }

   /// <summary>
   /// To int implicit conversion.
   /// </summary>
   /// <param name="value">The entity.</param>
   public static implicit operator int(DefaultIntId value)
   {
      return value.Value;
   }

   /// <summary>
   /// Converts the entity to int.
   /// </summary>
   /// <returns>The value object value.</returns>
   public int ToInt32()
   {
      return this.Value;
   }
}
