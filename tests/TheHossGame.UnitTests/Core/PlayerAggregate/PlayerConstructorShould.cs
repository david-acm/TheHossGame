// 🃏 The HossGame 🃏
// <copyright file="PlayerConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.UnitTests.Core.Services;
using Xunit;

public class PlayerConstructorShould
{
   [Fact]
   public void DeriveFromEntityBase() => typeof(Player).Should()
      .BeDerivedFrom<EntityBase<PlayerId>>();

   [Theory]
   [AutoPlayerData]
   public void CreateNewPlayer(Player player) => player.Should().NotBeNull();
}
