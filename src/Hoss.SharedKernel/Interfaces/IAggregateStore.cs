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
    /// <typeparam name="TId">The type of the aggregate.</typeparam>
    /// <typeparam name="T">The type of the aggregate expected to load</typeparam>
    Task<T> LoadAsync<T, TId>(TId id)
        where TId : ValueId
        where T : AggregateRoot<TId>;

    /// <summary>
    ///     Checks if an aggregate with a given id exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <returns></returns>
    Task<bool> Exists<T, TId>(TId id)
        where TId : ValueId
        where T : AggregateRoot<TId>;

    /// <summary>
    /// Saves an aggregate by persisting all its events to the store
    /// </summary>
    /// <param name="aggregate"> The aggregate to save</param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <returns></returns>
    Task Save<T, TId>(T aggregate)
        where TId : ValueId
        where T : AggregateRoot<TId>;
}