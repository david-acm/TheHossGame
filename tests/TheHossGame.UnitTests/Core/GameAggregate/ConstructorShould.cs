// 🃏 The HossGame 🃏
// <copyright file="ConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate;

using FluentAssertions;
using TheHossGame.SharedKernel.Interfaces;
using Xunit;

public class ConstructorShould
{
   public ConstructorShould()
   {
   }

   [Fact]
   public void ReturnNewGameAggregate()
   {
      var game = new Game();

      game.Should().NotBeNull();
      typeof(Game).Should().Implement<IAggregateRoot>();
   }
}
