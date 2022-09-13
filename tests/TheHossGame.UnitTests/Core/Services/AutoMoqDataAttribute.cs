// 🃏 The HossGame 🃏
// <copyright file="AutoMoqDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System.Collections.ObjectModel;
using TheHossGame.UnitTests.Core.PlayerAggregate;

#pragma warning disable CA1813 // Avoid unsealed attributes
public class CustomDataAttribute : AutoDataAttribute
#pragma warning restore CA1813 // Avoid unsealed attributes
{
   public CustomDataAttribute()
      : base(FixtureFactory)
   {
   }

   public static Func<IFixture> FixtureFactory { get; private set; } = () =>
   {
      var fixture = new Fixture();
      Customizations!.ToList().ForEach(
               c => fixture.Customize(c));
      return fixture;
   };

   protected static Collection<ICustomization> Customizations { get; } =
      new Collection<ICustomization>();
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoMoqDataAttribute : CustomDataAttribute
{
   public AutoMoqDataAttribute()
   {
      Customizations.Add(new AutoMoqCustomization());
   }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoPlayerDataAttribute : CustomDataAttribute
{
   public AutoPlayerDataAttribute()
   {
      Customizations.Add(new PlayerDataCustomization());
   }
}

public class PlayerDataCustomization : ICustomization
{
   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new PlayerNameGenerator());
   }
}
