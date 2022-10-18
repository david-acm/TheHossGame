// ---
// 🃏 The HossGame 🃏
// <copyright file="DomainEventBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using MediatR;

/// <summary>
///    Base class for domain events.
/// </summary>
public record DomainEventBase : INotification
{
   /// <summary>
   ///    Initializes a new instance of the <see cref="DomainEventBase" /> class.
   /// </summary>
   /// <param name="entityId">The entity id.</param>
   protected DomainEventBase(ValueObject entityId)
   {
      this.EntityId = entityId;
      this.EntityId = entityId;
   }

   /// <summary>
   ///    Gets the entity id.
   /// </summary>
   private ValueObject EntityId { get; }

   /// <summary>
   ///    Gets the date the event occurred on in UTC.
   /// </summary>
   public DateTime DateOccurred { get; init; }
}
