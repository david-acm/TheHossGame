// 🃏 The HossGame 🃏
// <copyright file="PlayerConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

#region

using FluentAssertions;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

#endregion

public class PlayerConstructorShould
{
   [Fact]
   public void DeriveFromEntityBase()
   {
      typeof(APlayer).Should().BeDerivedFrom<EntityBase<PlayerId>>();
   }

   [Theory]
   [AutoPlayerData]
   public void CreateNewPlayer(APlayer player)
   {
      player.Should().NotBeNull();
   }
}
