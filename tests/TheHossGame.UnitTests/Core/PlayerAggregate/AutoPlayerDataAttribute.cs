// 🃏 The HossGame 🃏
// <copyright file="AutoPlayerDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using AutoFixture;
using TheHossGame.UnitTests.Core.Services;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoPlayerDataAttribute : LazyDataAttribute
{
   public AutoPlayerDataAttribute()
   {
      AddCustomization(new PlayerDataCustomization());
   }
}

internal class PlayerDataCustomization : ICustomization
{
   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new PlayerNameGenerator());
      fixture.Customizations.Add(new PlayerEmailGenerator());
   }
}