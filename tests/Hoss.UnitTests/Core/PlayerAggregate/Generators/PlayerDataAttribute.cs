// 🃏 The HossGame 🃏
// <copyright file="AutoPlayerDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.AutoMoq;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.UnitTests.Core.Services;

#endregion

[AttributeUsage(AttributeTargets.Method)]
public class PlayerDataAttribute : LazyDataAttribute
{
    public PlayerDataAttribute()
    {
        AddCustomization(new PlayerDataCustomization());
        AddCustomization(new AutoMoqCustomization());
        AddCustomization(new BidValueCustomization());
    }
}

internal class PlayerDataCustomization : ICustomization
{
    #region ICustomization Members

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new PlayerNameGenerator());
        fixture.Customizations.Add(new PlayerEmailGenerator());
        fixture.Customizations.Add(new PlayerGenerator());
        fixture.Customizations.Add(new PlayerEnumerableGenerator());
    }

    #endregion
}

internal class ReadyGameCustomization : ICustomization
{
    #region ICustomization Members

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new PlayerNameGenerator());
        fixture.Customizations.Add(new PlayerGenerator());
        fixture.Customizations.Add(new PlayerEnumerableGenerator());
        fixture.Customizations.Add(new ReadyGameGenerator());
        fixture.Customize(new BidValueCustomization());
    }

    #endregion
}

internal class BidValueCustomization : ICustomization
{
    #region ICustomization Members

    /// <inheritdoc />
    public void Customize(IFixture fixture)
    {
        fixture.Customize<BidValue>(c => c.FromFactory(() =>
        {
            var random = new Random();
            return BidValue.FromValue(random.Next(0, 6));
        }));
    }

    #endregion
}

internal class ReadyBidFinishedGameCustomization : ICustomization
{
    #region ICustomization Members

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new PlayerNameGenerator());
        fixture.Customizations.Add(new PlayerGenerator());
        fixture.Customizations.Add(new PlayerEnumerableGenerator());
        fixture.Customizations.Add(new BidFinishedGameGenerator());
    }

    #endregion
}

internal class BidWithHossGameCustomization : ICustomization
{
    #region ICustomization Members

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new PlayerNameGenerator());
        fixture.Customizations.Add(new PlayerGenerator());
        fixture.Customizations.Add(new PlayerEnumerableGenerator());
        fixture.Customizations.Add(new BidWithHossGameGenerator());
    }

    #endregion
}