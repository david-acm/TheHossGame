// 🃏 The HossGame 🃏
// <copyright file="IEventStore.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel.Interfaces;

/// <summary>
///    Provides methods to retrieve aggregate roots from an event-store.
/// </summary>
public interface IAggregateStore
{
    /// <summary>
    ///    Gets an aggregate root by its Id.
    /// </summary>
    /// <returns>The aggregate root.</returns>
    /// <param name="id">The id of the aggregate to get.</param>
    /// <typeparam name="T">The type of the aggregate expected to load</typeparam>
    Task<T> LoadAsync<T>(Guid id)
        where T : IAggregateRoot;

    /// <summary>
    ///     Checks if an aggregate with a given id exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> Exists<T>(Guid id)
        where T : IAggregateRoot;

    /// <summary>
    /// Saves an aggregate by persisting all its events to the store
    /// </summary>
    /// <param name="aggregate"> The aggregate to save</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SaveAsync<T>(T aggregate)
        where T : IAggregateRoot;
}