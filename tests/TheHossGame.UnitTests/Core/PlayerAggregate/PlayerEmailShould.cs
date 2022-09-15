// 🃏 The HossGame 🃏
// <copyright file="PlayerEmailShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using Xunit;

public class PlayerEmailShould
{
   [Theory]
   [AutoPlayerData]
   public void HaveValueComparison(PlayerEmail email)
   {
      var otherEmail = PlayerEmail.FromString(email.Address);

      (email == otherEmail).Should().BeTrue();
   }

   [Fact]
   public void BeImmutable()
   {
      var customAttributes = typeof(PlayerEmail).GetTypeInfo().DeclaredProperties
         .SelectMany(p => p.GetCustomAttributes(true));
      var isRecord = customAttributes.FirstOrDefault() is CompilerGeneratedAttribute;

      isRecord.Should().BeTrue();
      typeof(PlayerEmail).Should().BeAssignableTo<ValueObject>();
   }

   [Theory]
   [AutoPlayerData]
   public void NotBeNull(PlayerEmail email) => email.Should().NotBeNull();

   [Theory]
   [AutoData]
   public void ThrowArgumentExceptionWhenEmailIsNotValid(string emailAddress)
   {
      var playerEmail = () => PlayerEmail.FromString(emailAddress);

      playerEmail.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void ThrowArgumentNullExceptionWhenEmailIsNull()
   {
      var playerEmail = () => PlayerEmail.FromString(null!);

      playerEmail.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void ThrowArgumentNullExceptionWhenEmailIsEmpty()
   {
      var playerEmail = () => PlayerEmail.FromString(string.Empty);

      playerEmail.Should().Throw<ArgumentException>();
   }
}
