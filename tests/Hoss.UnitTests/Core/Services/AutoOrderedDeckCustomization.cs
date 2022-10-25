// 🃏 The HossGame 🃏
// <copyright file="AutoOrderedDeckCustomization.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

   #region

using AutoFixture;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#endregion

internal class AutoOrderedDeckCustomization : ICustomization
{
   #region ICustomization Members

   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new AutoOrderedDeckGenerator());
      fixture.Customizations.Add(new AutoShufflingServiceGenerator());
      fixture.Customizations.Add(new PlayerEnumerableGenerator());
   }

   #endregion
}

internal class AutoPlayerWithNoTrumpCustomization : ICustomization
{
   #region ICustomization Members

   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new AutoHossShufflingServiceGenerator());
      fixture.Customizations.Add(new PlayerEnumerableGenerator());
   }

   #endregion
}
