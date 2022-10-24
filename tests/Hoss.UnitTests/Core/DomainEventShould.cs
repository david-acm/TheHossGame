// 🃏 The HossGame 🃏
// <copyright file="DomainEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core;

#region

using System.Reflection;
using FluentAssertions.Types;
using Hoss.Core.PlayerAggregate.Events;
using Hoss.SharedKernel;
using Xunit;

#endregion

public class DomainEventShould
{
   [Fact]
   public void SetMethodsShouldBePrivate()
   {
      var eventsAssembly = typeof(PlayerRegisteredEvent).Assembly;
      var properties = AllTypes.From(eventsAssembly).ThatDeriveFrom<DomainEventBase>().SelectMany(v => v.GetProperties());
      var setMethods = properties.Select(p => p.SetMethod).OfType<MethodInfo>().Where(p => p.DeclaringType != typeof(DomainEventBase) && !p.CustomAttributes.Any()).ToList();
   }
}
