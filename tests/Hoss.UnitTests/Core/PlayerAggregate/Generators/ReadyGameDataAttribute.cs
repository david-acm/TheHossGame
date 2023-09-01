// 🃏 The HossGame 🃏
// <copyright file="AutoReadyGameDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using TheHossGame.UnitTests.Core.Services;

#endregion

public sealed class ReadyGameDataAttribute : LazyDataAttribute
{
    public ReadyGameDataAttribute()
    {
        AddCustomization(new ReadyGameCustomization());
        AddCustomization(new AutoOrderedDeckCustomization());
    }
}

public sealed class BidFinishedGameDataAttribute : LazyDataAttribute
{
    public BidFinishedGameDataAttribute()
    {
        AddCustomization(new ReadyBidFinishedGameCustomization());
        AddCustomization(new AutoOrderedDeckCustomization());
        AddCustomization(new BidValueCustomization());
    }
}

public sealed class BidWithHossGameDataAttribute : LazyDataAttribute
{
    public BidWithHossGameDataAttribute()
    {
        AddCustomization(new BidWithHossGameCustomization());
        AddCustomization(new AutoOrderedDeckCustomization());
        AddCustomization(new BidValueCustomization());
    }
}

public sealed class HossRoundDataAttribute : LazyDataAttribute
{
    public HossRoundDataAttribute()
    {
        AddCustomization(new ReadyBidFinishedGameCustomization());
        AddCustomization(new AutoPlayerWithNoTrumpCustomization());
    }
}