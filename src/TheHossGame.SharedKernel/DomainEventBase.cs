// ---
// 🃏 The HossGame 🃏
// <copyright file="DomainEventBase.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

using MediatR;
using System;

/// <summary>
/// Base class for domain events.
/// </summary>
public record DomainEventBase : INotification
{
   /// <summary>
   /// Gets the date the event ocurred on in UTC.
   /// </summary>
   public DateTime DateOccurred { get; init; }
}