// 🃏 The HossGame 🃏
// <copyright file="AutoMoqDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

#region

using AutoFixture.AutoMoq;

#endregion

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoMoqDataAttribute : LazyDataAttribute
{
   public AutoMoqDataAttribute()
   {
      AddCustomization(new AutoMoqCustomization());
   }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoOrderedDeckDataAttribute : LazyDataAttribute
{
   public AutoOrderedDeckDataAttribute()
   {
      AddCustomization(new AutoMoqCustomization());
      AddCustomization(new AutoOrderedDeckCustomization());
   }
}
