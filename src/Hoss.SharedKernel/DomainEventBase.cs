// 🃏 The HossGame 🃏
// <copyright file="DomainEventBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

#region

using MediatR;

#endregion

/// <summary>
///    Base class for domain events.
/// </summary>
public record DomainEventBase(ValueObject EntityId) : INotification
{
   /// <summary>
   ///    Gets the date the event occurred on in UTC.
   /// </summary>
   public DateTime DateOccurred { get; init; }
}
