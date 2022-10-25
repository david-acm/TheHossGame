﻿// 🃏 The HossGame 🃏
// <copyright file="AutoPlayerDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.AutoMoq;
using TheHossGame.UnitTests.Core.Services;

#endregion

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoPlayerDataAttribute : LazyDataAttribute
{
   public AutoPlayerDataAttribute()
   {
      AddCustomization(new PlayerDataCustomization());
      AddCustomization(new AutoMoqCustomization());
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