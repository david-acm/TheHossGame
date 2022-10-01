// 🃏 The HossGame 🃏
// <copyright file="LazyDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture;
using AutoFixture.Xunit2;
public abstract class LazyDataAttribute : AutoDataAttribute
{
   protected LazyDataAttribute()
      : base(ApplyCustomizations)
   {
      Customizations.Clear();
   }

   public static Func<IFixture> ApplyCustomizations { get; private set; } = () =>
   {
      var fixture = new Fixture();
      Customizations!.ToList().ForEach(
               c => fixture.Customize(c));
      return fixture;
   };

   private static List<ICustomization> Customizations { get; } =
      new List<ICustomization>();

   public static void AddCustomization(ICustomization customization) =>
      Customizations.Add(customization);
}
