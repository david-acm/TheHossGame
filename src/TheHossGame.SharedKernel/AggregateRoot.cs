// 🃏 The HossGame 🃏
// <copyright file="AggregateRoot.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.SharedKernel;

/// <summary>
/// The aggregate root base class.
/// </summary>
/// <typeparam name="TId">The identity value.</typeparam>
public class AggregateRoot<TId> : EntityBase<TId>
   where TId : ValueId
{
   /// <summary>
   /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
   /// </summary>
   /// <param name="id">The identity value.</param>
   public AggregateRoot(TId id)
      : base(id)
   {
   }

   /// <inheritdoc/>
   protected override void EnsureValidState()
   {
      throw new NotImplementedException();
   }

   /// <inheritdoc/>
   protected override void When(DomainEventBase @event)
   {
      throw new NotImplementedException();
   }

   /// <summary>
   /// Applies an event to an entity.
   /// </summary>
   /// <param name="entity">The entity to apply the event.</param>
   /// <param name="event">The event to apply.</param>
   protected void ApplyToEntity(IInternalEventHandler entity, DomainEventBase @event) => entity?.Handle(@event);

   /// <summary>
   /// Aplies an event to an aggregate root.
   /// </summary>
   /// <param name="event">The event to apply.</param>
   protected override void Apply(DomainEventBase @event)
   {
      this.When(@event);
      this.EnsureValidState();
      this.RaiseDomainEvent(@event);
   }
}
