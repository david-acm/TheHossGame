﻿// ---
// 🃏 The HossGame 🃏
// <copyright file="EntityBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Base class for entities.
/// </summary>
public abstract class EntityBase : EntityBase<DefaultIntId>
{
   /// <summary>
   /// Initializes a new instance of the <see cref="EntityBase"/> class.
   /// </summary>
   /// <param name="id">The entity id.</param>
   protected EntityBase(int id)
      : base(new DefaultIntId(id))
   {
   }

   /// <summary>
   /// Initializes a new instance of the <see cref="EntityBase"/> class.
   /// </summary>
   protected EntityBase()
      : base(new DefaultIntId(0))
   {
   }

   /// <summary>
   /// Gets or sets the id value.
   /// </summary>
   public int IdValue
   {
      get
      {
         return this.Id.Value;
      }

      set
      {
         this.Id.Value = value;
      }
   }
}

/// <summary>
/// Base class for entities.
/// </summary>
/// <typeparam name="T">The id type.</typeparam>
public abstract class EntityBase<T>
   where T : ValueObject
{
   private readonly List<DomainEventBase> domainEvents = new ();

   /// <summary>
   /// Initializes a new instance of the <see cref="EntityBase{T}"/> class.
   /// </summary>
   /// <param name="id">The entity id.</param>
   protected EntityBase(T id)
   {
      this.Id = id;
   }

   /// <summary>
   /// Gets the id.
   /// </summary>
   public T Id { get; private set; }

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
