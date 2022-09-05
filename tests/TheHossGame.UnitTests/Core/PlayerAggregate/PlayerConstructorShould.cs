// 🃏 The HossGame 🃏
// <copyright file="PlayerConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using Xunit;

public class PlayerConstructorShould
{
   public PlayerConstructorShould()
   {
   }

   [Theory]
   [AutoData]
   public void CreateNewPlayer(Player player) => player.Should().NotBeNull();

   [Fact]
   public void ThrowArgumentNullExceptionWhenNameIsNull()
   {
      var fixture = new Fixture();
      var playerCreation = () => new Player(
         null!,
         fixture.Create<PlayerEmail>());

      var exception = playerCreation.Should().Throw<ArgumentException>();
      exception.WithMessage("Value cannot be null. (Parameter 'name')");
   }

   [Fact]
   public void ThrowArgumentNullExceptionWhenEmailIsNull()
   {
      var fixture = new Fixture();
      var playerCreation = () => new Player(
         fixture.Create<PlayerName>(),
         null!);

      var exception = playerCreation.Should().Throw<ArgumentException>();
      exception.WithMessage("Value cannot be null. (Parameter 'email')");
   }
}
