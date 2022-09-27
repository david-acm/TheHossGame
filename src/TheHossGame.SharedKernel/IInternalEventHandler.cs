// 🃏 The HossGame 🃏
// <copyright file="IInternalEventHandler.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.SharedKernel;

/// <summary>
/// Provides the avility to handle aggregate events.
/// </summary>
public interface IInternalEventHandler
{
   /// <summary>
   /// Handles an event.
   /// </summary>
   /// <param name="event">The event to handler.</param>
   void Handle(DomainEventBase @event);
}