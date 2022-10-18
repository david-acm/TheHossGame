// ---
// 🃏 The HossGame 🃏
// <copyright file="EntityBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
///    Base class for entities.
/// </summary>
public abstract class EntityBase : EntityBase<DefaultIntId>
{
   /// <summary>
   ///    Initializes a new instance of the <see cref="EntityBase" /> class.
   /// </summary>
   /// <param name="id">The entity id.</param>
   protected EntityBase(int id)
      : base(new DefaultIntId(id))
   {
   }

   /// <summary>
   ///    Initializes a new instance of the <see cref="EntityBase" /> class.
   /// </summary>
   protected EntityBase()
      : base(new DefaultIntId(0))
   {
   }

   /// <summary>
   ///    Gets the id value.
   /// </summary>
   public int IdValue => this.Id.Value;
}

/// <summary>
///    Base class for entities.
/// </summary>
/// <typeparam name="T">The id type.</typeparam>
public abstract class EntityBase<T> : IInternalEventHandler
   where T : ValueObject
{
   private readonly List<DomainEventBase> domainEvents = new ();

   /// <summary>
   ///    Initializes a new instance of the <see cref="EntityBase{T}" /> class.
   /// </summary>
   /// <param name="id">The entity id.</param>
   protected EntityBase(T id)
   {
      this.Id = id;
      this.Applier = this.RaiseDomainEvent;
   }

   /// <summary>
   ///    Initializes a new instance of the <see cref="EntityBase{T}" /> class.
   /// </summary>
   /// <param name="id">The entity id.</param>
   /// <param name="applier">The event applier.</param>
   protected EntityBase(T id, Action<DomainEventBase> applier)
   {
      this.Id = id;
      this.Applier = applier;
   }

   /// <summary>
   ///    Gets the id.
   /// </summary>
   public T Id { get; }

   /// <summary>
   ///    Gets a readonly collection of domain events.
   /// </summary>
   [NotMapped]
   public IEnumerable<DomainEventBase> Events => this.domainEvents.AsReadOnly();

   /// <summary>
   ///    Gets a value indicating whether the entity is null.
   /// </summary>
   protected abstract bool IsNull { get; }

   /// <summary>
   ///    Gets the event applier.
   /// </summary>
   protected Action<DomainEventBase> Applier { get; }

   /// <summary>
   ///    Performs identity based comparison.
   /// </summary>
   /// <param name="obj">The object to compare.</param>
   /// <returns>Whether the two objects are equal.</returns>
   public override bool Equals(object? obj)
   {
      if (obj is not EntityBase other)
      {
         return false;
      }

      if (ReferenceEquals(this, other))
      {
         return true;
      }

      if (this.GetType() != other.GetType())
      {
         return false;
      }

      return this.Id == other.Id;
   }

   /// <summary>
   ///    Gets the hash code of other entity.
   /// </summary>
   /// <returns>The hash code of the entity.</returns>
   public override int GetHashCode()
   {
      return (this.GetType().ToString() + this.Id).GetHashCode(StringComparison.InvariantCulture);
   }

   /// <summary>
   ///    Clears the collection of domain events.
   /// </summary>
   internal void ClearDomainEvents()
   {
      this.domainEvents.Clear();
   }

   /// <summary>
   ///    Applies an event to an entity.
   /// </summary>
   /// <param name="event">The event to apply.</param>
   protected virtual void Apply(DomainEventBase @event)
   {
      if (this.IsNull)
      {
         return;
      }

      this.When(@event);
      this.EnsureValidState();
      this.Applier(@event);
   }

   /// <summary>
   ///    Applies concrete event to entity.
   /// </summary>
   /// <param name="event">The event to apply.</param>
   protected abstract void When(DomainEventBase @event);

   /// <summary>
   ///    Ensures the entity is in a valid state.
   /// </summary>
   protected abstract void EnsureValidState();

   /// <summary>
   ///    Adds a new domain event to the collection.
   /// </summary>
   /// <param name="domainEvent">The domain event to be added.</param>
#pragma warning disable CA1030
   protected void RaiseDomainEvent(DomainEventBase domainEvent)
#pragma warning restore CA1030
   {
      this.domainEvents.Add(domainEvent);
   }
}
