// 🃏 The HossGame 🃏
// <copyright file="ValueObjectShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core;

using System.Reflection;
using System.Runtime.CompilerServices;
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
      var properties =
         GetValueTypesFromAssembly()
            .SelectMany(v => v.GetProperties());
      var setMethods = properties.Select(p => p.SetMethod).OfType<MethodInfo>().ToList();

      setMethods.ForEach(
         m =>
         {
            var privateOrInit = m.IsPrivate || m.IsHideBySig;
            privateOrInit.Should().BeTrue(
               $"{m.DeclaringType?.FullName} 👉 {m.Name} method should be private.");
         });
   }

   [Fact]
   public void HaveReadOnlyProperties()
   {
      var properties =
         GetValueTypesFromAssembly()
            .SelectMany(v => v.GetProperties());
      var readOnlyProperties = properties
                               .Where(p => p.SetMethod is null)
                               .ToList();

      readOnlyProperties.ForEach(
         p => p.CanWrite.Should().BeFalse(
            $"{p.DeclaringType?.FullName} 👉 {p.Name} property should be readOnly."));
   }

   [Fact]
   public void BeImmutable()
   {
      var types = GetValueTypesFromAssembly();

      types.ForEach(type => IsRecord(type).Should().BeTrue());
   }

   private static List<Type> GetValueTypesFromAssembly()
   {
      var assembly = typeof(PlayerName).Assembly;
      List<Type> types =
         AllTypes.From(assembly)
                 .ThatDeriveFrom<ValueObject>()
                 .ToList();
      return types;
   }

   private static bool IsRecord(Type t)
   {
      var customAttributes = t.GetTypeInfo().DeclaredProperties
                              .SelectMany(p => p.GetCustomAttributes(true));
      return customAttributes.FirstOrDefault() is CompilerGeneratedAttribute;
   }
}
