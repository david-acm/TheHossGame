﻿// 🃏 The HossGame 🃏
// <copyright file="IDomainEventDispatcher.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel.Interfaces;

/// <summary>
///    Dispatches domain events.
/// </summary>
public interface IDomainEventDispatcher<in T>
    where T : IInternalEventHandler
{
    /// <summary>
    ///    Dispatches all domain events in the entities.
    /// </summary>
    /// <param name="entitiesWithEvents">The entities with the events to dispatch.</param>
    /// <returns>The task.</returns>
    Task DispatchAndClearEvents(IEnumerable<T> entitiesWithEvents);
}