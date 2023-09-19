// 🃏 The HossGame 🃏
// <copyright file="LazyDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

#region

using AutoFixture;
using AutoFixture.Xunit2;

#endregion

public abstract class LazyDataAttribute : AutoDataAttribute
{
   protected LazyDataAttribute()
      : base(ApplyCustomizations)
   {
      Customizations.Clear();
   }

   private static Func<IFixture> ApplyCustomizations { get; } = () =>
   {
      var fixture = new Fixture();
      Customizations?.ToList().ForEach(c => fixture.Customize(c));
      return fixture;
   };

   private static List<ICustomization> Customizations { get; } = new ();

   protected static void AddCustomization(ICustomization customization)
   {
      Customizations.Add(customization);
   }
}
