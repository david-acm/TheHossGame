// 🃏 The HossGame 🃏
// <copyright file="AggregateRoot.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

/// <summary>
///    The aggregate root base class.
/// </summary>
/// <typeparam name="TId">The identity value.</typeparam>
public abstract class AggregateRoot<TId> : EntityBase<TId>
   where TId : ValueId
{
   /// <summary>
   ///    Initializes a new instance of the <see cref="AggregateRoot{TId}" /> class.
   /// </summary>
   /// <param name="id">The identity value.</param>
   protected AggregateRoot(TId id)
      : base(id)
   {
   }

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
      this.When(@event);
      this.EnsureValidState();
      this.RaiseDomainEvent(@event);
   }
}
