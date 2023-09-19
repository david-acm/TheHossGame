// 🃏 The HossGame 🃏
// <copyright file="EntityBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

/// <summary>
///    Base class for entities.
/// </summary>
public abstract class EntityBase : IInternalEventHandler
{
    private readonly List<DomainEventBase> domainEvents = new();

    /// <summary>
    ///    Initializes a new instance of the <see cref="EntityBase" /> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    protected EntityBase(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Applier = RaiseDomainEvent;
    }

    /// <summary>
    ///    Initializes a new instance of the <see cref="EntityBase" /> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    /// <param name="applier">The event applier.</param>
    protected EntityBase(Guid? id, Action<DomainEventBase> applier)
    {
        Id = id ?? Guid.Empty;
        Applier = applier;
    }

    /// <summary>
    ///    Gets the id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    ///    Gets the event applier.
    /// </summary>
    protected Action<DomainEventBase> Applier { get; }

    #region IInternalEventHandler Members

    /// <summary>
    ///    Gets a readonly collection of domain events.
    /// </summary>
    [NotMapped]
    public IEnumerable<DomainEventBase> Events => domainEvents.AsReadOnly();

    /// <summary>
    ///    Clears the collection of domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    #endregion

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

        if (GetType() != other.GetType())
        {
            return false;
        }

        return
            GetHashCode() == other.GetHashCode() &&
            Id == other.Id;
    }

    /// <summary>
    ///    Gets the hash code of other entity.
    /// </summary>
    /// <returns>The hash code of the entity.</returns>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode(StringComparison.InvariantCulture);
    }

    /// <summary>
    ///    Applies an event to an entity.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    protected virtual void Apply(DomainEventBase @event)
    {
        When(@event);
        EnsureValidState();
        Applier(@event);
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
        domainEvents.Add(domainEvent);
    }
}