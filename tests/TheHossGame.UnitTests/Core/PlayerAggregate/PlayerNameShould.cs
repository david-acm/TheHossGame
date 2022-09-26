// 🃏 The HossGame 🃏
// <copyright file="PlayerNameShould.cs" company="Reactive">
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
using Xunit;

public class PlayerNameShould
{
   [Theory]
   [AutoData]
   public void NotBeLongerThan30Characters([Frozen] string name)
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

   [Theory]
   [AutoPlayerData]
   public void HaveValueComparison([Frozen] PlayerName player1)
   {
      var player2 = new PlayerName(player1.Name);

      (player1 == player2).Should().BeTrue();
      player1.Equals(player2).Should().BeTrue();
      player1.Should().BeEquivalentTo(player2);
   }

   [Fact]
   public void BeAnImmutableObject()
   {
      var customAttributes = typeof(PlayerName).GetTypeInfo().DeclaredProperties
         .SelectMany(p => p.GetCustomAttributes(true));
      var isRecord = customAttributes.FirstOrDefault() is CompilerGeneratedAttribute;

      isRecord.Should().BeTrue();
   }

   [Fact]
   public void ThrowArgumentNullExceptionWhenNameIsEmpty()
   {
      var playerName = () => new PlayerName(string.Empty);

      playerName.Should().Throw<ArgumentException>();
   }
}
