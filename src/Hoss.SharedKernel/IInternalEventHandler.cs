// 🃏 The HossGame 🃏
// <copyright file="IInternalEventHandler.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

/// <summary>
///    Provides the ability to handle aggregate events.
/// </summary>
#pragma warning disable CA1040
#pragma warning disable CA1711
public interface IInternalEventHandler
#pragma warning restore CA1711
#pragma warning restore CA1040
{
    /// <summary>
    ///     Gets a readonly collection of domain events.
    /// </summary>
    IEnumerable<DomainEventBase> Events { get; }

    /// <summary>
    ///     Clears the collection of domain events.
    /// </summary>
    void ClearDomainEvents();
}