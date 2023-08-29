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

public sealed class AutoReadyGameDataAttribute : LazyDataAttribute
{
    public AutoReadyGameDataAttribute()
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