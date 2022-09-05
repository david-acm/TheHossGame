// 🃏 The HossGame 🃏
// <copyright file="PlayerNameShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using FluentAssertions;
using System;
using TheHossGame.Core.PlayerAggregate;
using Xunit;

public class PlayerNameShould
{
   public PlayerNameShould()
   {
   }

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
