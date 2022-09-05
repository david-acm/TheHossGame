// 🃏 The HossGame 🃏
// <copyright file="PlayerEmailShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using Xunit;

public class PlayerEmailShould
{
   [Fact]
   public void ThrowArgumentNullExceptionWhenEmailIsNull()
   {
      var playerEmail = () => new PlayerEmail(null!);

      playerEmail.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void ThrowArgumentNullExceptionWhenEmailIsEmpty()
   {
      var playerEmail = () => new PlayerEmail(string.Empty);

      playerEmail.Should().Throw<ArgumentException>();
   }
}
