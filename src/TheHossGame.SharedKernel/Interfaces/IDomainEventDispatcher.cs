// ---
// 🃏 The HossGame 🃏
// <copyright file="IDomainEventDispatcher.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel.Interfaces;

/// <summary>
/// Dispathces domain events.
/// </summary>
public interface IDomainEventDispatcher
{
  /// <summary>
  /// Dispatches all domain events in the entities.
  /// </summary>
  /// <param name="entitiesWithEvents">The entities with the events to dispatch.</param>
  /// <returns>The task.</returns>
  Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents);
}
