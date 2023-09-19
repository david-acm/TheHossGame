// 🃏 The HossGame 🃏
// <copyright file="PlayerConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.ProfileAggregate;

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

#region

using FluentAssertions;
using Hoss.SharedKernel;
using Generators;
using Xunit;

#endregion

public class PlayerConstructorShould
{
    [Fact]
    public void DeriveFromEntityBase()
    {
        typeof(Profile).Should().BeDerivedFrom<EntityBase>();
    }

    [Theory]
    [PlayerData]
    public void CreateNewPlayer(Profile profile)
    {
        profile.Should().NotBeNull();
    }
}