// 🃏 The HossGame 🃏
// <copyright file="AutoReadyGameDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using TheHossGame.UnitTests.Core.Services;

public sealed class AutoReadyGameDataAttribute : LazyDataAttribute
{
   public AutoReadyGameDataAttribute()
   {
      AddCustomization(new ReadyGameCustomization());
      AddCustomization(new AutoOrderedDeckCustomization());
   }
}

public sealed class AutoBidFinishedGameDataAttribute : LazyDataAttribute
{
   public AutoBidFinishedGameDataAttribute()
   {
      AddCustomization(new ReadyBidFinishedGameCustomization());
      AddCustomization(new AutoOrderedDeckCustomization());
   }
}
