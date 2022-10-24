// 🃏 The HossGame 🃏
// <copyright file="ConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.SharedKernel.Interfaces;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

#endregion

public class ConstructorShould
{
   [Theory]
   [AutoPlayerData]
   public void ReturnNewGameAggregate(AGame game)
   {
      game.Should().NotBeNull();

      typeof(AGame).Should().Implement<IAggregateRoot>();
   }
}
