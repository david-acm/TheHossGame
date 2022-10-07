// 🃏 The HossGame 🃏
// <copyright file="AutoOrderedDeckCustomization.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

internal class AutoOrderedDeckCustomization : ICustomization
{
   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new AutoOrderedDeckGenerator());
      fixture.Customizations.Add(new AutoShufflingServiceGenerator());
      fixture.Customizations.Add(new PlayerEnumerableGenerator());
   }
}
