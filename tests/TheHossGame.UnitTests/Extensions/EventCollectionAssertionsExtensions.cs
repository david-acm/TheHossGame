// 🃏 The HossGame 🃏
// <copyright file="EventCollectionAssertionsExtensions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Extensions;

using TheHossGame.SharedKernel;

public static class EventCollectionAssertionsExtensions
{
   public static EventCollectionAssertions<TEvent> ShouldContain<TEvent>(this IEnumerable<TEvent> instance)
      where TEvent : DomainEventBase
   {
      return new EventCollectionAssertions<TEvent>(instance);
   }
}
