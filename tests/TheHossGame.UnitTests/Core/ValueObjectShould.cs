// 🃏 The HossGame 🃏
// <copyright file="ValueObjectShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Types;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using Xunit;

public class ValueObjectShould
{
   [Fact]
   public void SetMethodsShouldBePrivate()
   {
      var assembly = typeof(PlayerName).Assembly;
      IEnumerable<PropertyInfo> properties =
         AllTypes.From(assembly)
                 .ThatDeriveFrom<ValueObject>()
                 .SelectMany(v => v.GetProperties());
      var setMethods = properties.Select(p => p.SetMethod).OfType<MethodInfo>().ToList();

      setMethods.ForEach(m =>
         m.IsPrivate.Should().BeTrue(
            because: $"{m?.DeclaringType?.FullName} 👉 {m?.Name} method should be private."));
   }

   [Fact]
   public void HaveReadOnlyProperties()
   {
      var assembly = typeof(PlayerName).Assembly;
      IEnumerable<PropertyInfo> properties = AllTypes
         .From(assembly)
         .ThatDeriveFrom<ValueObject>()
         .SelectMany(v => v.GetProperties());
      var readOnlyProperties = properties
         .Where(p => p.SetMethod is null)
         .ToList();

      readOnlyProperties.ForEach(p => p.CanWrite.Should().BeFalse(
         because: $"{p?.DeclaringType?.FullName} 👉 {p?.Name} property should be readOnly."));
   }
}
