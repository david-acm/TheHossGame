// 🃏 The HossGame 🃏
// <copyright file="AggregateRoot.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.SharedKernel.Interfaces;

namespace Hoss.SharedKernel;

/// <summary>
///    The aggregate root base class.
/// </summary>
public abstract class AggregateRoot : EntityBase, IAggregateRoot
{
   /// <summary>
   ///    Initializes a new instance of the <see cref="AggregateRoot" /> class.
   /// </summary>
   /// <param name="id">The identity value.</param>
   protected AggregateRoot(Guid? id)
      : base(id)
   {
   }

   /// <summary>
   /// 
   /// </summary>
   public ulong Version { get; private set; }
   
   /// <inheritdoc />
   protected abstract override void EnsureValidState();

   /// <inheritdoc />
   protected abstract override void When(DomainEventBase @event);

   /// <summary>
   ///    Applies an event to an aggregate root.
   /// </summary>
   /// <param name="event">The event to apply.</param>
   protected override void Apply(DomainEventBase @event)
   {
      When(@event);
      EnsureValidState();
      RaiseDomainEvent(@event);
   }

   /// <summary>
   /// Loads events to the aggregate.
   /// </summary>
   /// <param name="events"></param>
   public override void Load(IEnumerable<DomainEventBase> events)
   {
      foreach (var @event in events)
      {
         When(@event);
         Version++;
      }
   }
}
