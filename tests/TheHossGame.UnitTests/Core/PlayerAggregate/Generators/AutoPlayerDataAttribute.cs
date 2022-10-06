// 🃏 The HossGame 🃏
// <copyright file="AutoPlayerDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.AutoMoq;
using TheHossGame.UnitTests.Core.Services;

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
   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new PlayerNameGenerator());
      fixture.Customizations.Add(new PlayerEmailGenerator());
      fixture.Customizations.Add(new PlayerIdGenerator());
      fixture.Customizations.Add(new PlayerGenerator());
      fixture.Customizations.Add(new PlayerEnumerableGenerator());
   }
}

internal class ReadyGameCustomization : ICustomization
{
   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new PlayerNameGenerator());
      fixture.Customizations.Add(new PlayerIdGenerator());
      fixture.Customizations.Add(new PlayerGenerator());
      fixture.Customizations.Add(new PlayerEnumerableGenerator());
      fixture.Customizations.Add(new ReadyGameGenerator());
   }
}