// ---
// 🃏 The HossGame 🃏
// <copyright file="DomainEventBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using MediatR;

/// <summary>
/// Base class for domain events.
/// </summary>
public abstract class DomainEventBase : INotification
{
    /// <summary>
    /// Gets or sets the date the event ocurrend on in UTC.
    /// </summary>
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
