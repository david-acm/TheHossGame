// 🃏 The HossGame 🃏
// <copyright file="PlayerNameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using Xunit;

public class PlayerNameShould
{
   [Theory]
   [AutoData]
   public void NotBeLongerThan30Characters([Frozen] string name = "APlayer'sNameLongerThan30Characters")
   {
      var playerCreation = () => new PlayerName(name);

      playerCreation.Should().Throw<ArgumentException>();
   }

   [Theory]
   [AutoData]
   public void NotBeShorterThan2Characters([Frozen] string name = "A")
   {
      var playerCreation = () => new PlayerName(name);

      playerCreation.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void BeAValueObject() =>
      typeof(PlayerName).Should().BeDerivedFrom<ValueObject>();

   [Fact]
   public void ThrowArgumentExceptionWhenNameIsNull()
   {
      var playerName = () => new PlayerName(null!);

      playerName.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void ThrowArgumentNullExceptionWhenNameIsEmpty()
   {
      var playerName = () => new PlayerName(string.Empty);

      playerName.Should().Throw<ArgumentException>();
   }
}
