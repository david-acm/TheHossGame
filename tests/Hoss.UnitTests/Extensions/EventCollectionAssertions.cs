﻿// 🃏 The HossGame 🃏
// <copyright file="EventCollectionAssertions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Extensions;

#region

using FluentAssertions;
using FluentAssertions.Collections;
using Hoss.SharedKernel;

#endregion

public class EventCollectionAssertions<TEvent> : GenericCollectionAssertions<IEnumerable<TEvent>, TEvent>
   where TEvent : DomainEventBase
{
   public EventCollectionAssertions(IEnumerable<TEvent> instance)
      : base(instance)
   {
   }

   public TExpectedEvent SingleEventOfType<TExpectedEvent>(string because = "", params object[] becauseArgs)
      where TExpectedEvent : DomainEventBase
   {
      var @event = this.Subject.Where(e => e is TExpectedEvent).Should().ContainSingle(because, becauseArgs).Subject.As<TExpectedEvent>();

      return @event;
   }

   public void NoEventsOfType<TExpectedEvent>(string because = "", params object[] becauseArgs)
      where TExpectedEvent : DomainEventBase
   {
      this.Subject.Where(e => e is TExpectedEvent).Should().NotContain(e => e is TExpectedEvent, because, becauseArgs);
   }

   public IEnumerable<TExpectedEvent> ManyEventsOfType<TExpectedEvent>(int count, string because = "", params object[] becauseArgs)
      where TExpectedEvent : DomainEventBase
   {
      var @event = this.Subject.Where(e => e is TExpectedEvent).Should().HaveCount(count, because, becauseArgs).And.Subject.Cast<TExpectedEvent>().As<IEnumerable<TExpectedEvent>>();

      return @event;
   }
}