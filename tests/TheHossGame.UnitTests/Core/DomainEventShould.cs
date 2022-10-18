// 🃏 The HossGame 🃏
// <copyright file="DomainEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core;

using System.Reflection;
using FluentAssertions;
using FluentAssertions.Types;
using TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.SharedKernel;
using Xunit;

public class DomainEventShould
{
   [Fact]
   public void SetMethodsShouldBePrivate()
   {
      var eventsAssembly = typeof(PlayerRegisteredEvent).Assembly;
      var properties =
         AllTypes.From(eventsAssembly)
                 .ThatDeriveFrom<DomainEventBase>()
                 .SelectMany(v => v.GetProperties());
      var setMethods = properties.Select(p => p.SetMethod).OfType<MethodInfo>()
                                 .Where(
                                    p =>
                                       p.DeclaringType != typeof(DomainEventBase) &&
                                       !p.CustomAttributes.Any())
                                 .ToList();

      setMethods.ForEach(
         m =>
            m.IsPrivate.Should().BeTrue(
               $"{m.DeclaringType?.FullName} 👉 {m.Name} method should be private."));
   }
}
