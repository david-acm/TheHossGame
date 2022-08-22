// ---
// 🃏 The HossGame 🃏
// <copyright file="EntityBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using System.ComponentModel.DataAnnotations.Schema;

// This can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)

/// <summary>
/// Base class for entities.
/// </summary>
public abstract class EntityBase
{
    private readonly List<DomainEventBase> domainEvents = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase"/> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    protected EntityBase(int id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase"/> class.
    /// </summary>
    protected EntityBase()
    {
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets a readonly collection of domain events.
    /// </summary>
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => this.domainEvents.AsReadOnly();

    /// <summary>
    /// Clears the collection of domain events.
    /// </summary>
    internal void ClearDomainEvents() => this.domainEvents.Clear();

    /// <summary>
    /// Adds a new domain event to the collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    protected void RegisterDomainEvent(DomainEventBase domainEvent) => this.domainEvents.Add(domainEvent);
}
