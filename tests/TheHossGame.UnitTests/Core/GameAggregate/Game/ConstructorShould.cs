// 🃏 The HossGame 🃏
// <copyright file="ConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.SharedKernel.Interfaces;
using Xunit;

public class ConstructorShould
{
   public ConstructorShould()
   {
   }

   [Theory]
   [AutoData]
   public void ReturnNewGameAggregate(AGame game)
   {
      game.Should().NotBeNull();
      typeof(AGame).Should().Implement<IAggregateRoot>();
   }
}
