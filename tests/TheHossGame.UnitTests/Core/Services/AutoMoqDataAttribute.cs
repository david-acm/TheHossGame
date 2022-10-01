// 🃏 The HossGame 🃏
// <copyright file="AutoMoqDataAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Moq;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.RoundAggregate;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoMoqDataAttribute : LazyDataAttribute
{
   public AutoMoqDataAttribute() => AddCustomization(new AutoMoqCustomization());
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

internal class AutoOrderedDeckCustomization : ICustomization
{
   public void Customize(IFixture fixture)
   {
      fixture.Customizations.Add(new AutoOrderedDeckGenerator());
      fixture.Customizations.Add(new AutoShufflingServiceGenerator());
   }
}

public class AutoOrderedDeckGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(ADeck).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.GeneratePlayerEnumerable();
   }

   private object GeneratePlayerEnumerable()
   {
      var shufflingService = this.context.Create<Mock<IShufflingService>>();
      var cards = new List<Card>();
      shufflingService
         .Setup(s => s.Shuffle(It.IsAny<IList<Card>>()))
         .Callback((IList<Card> a) => cards = a.ToList());
      return ADeck.FromShuffling(shufflingService.Object);
   }
}

public class AutoShufflingServiceGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(Mock<IShufflingService>).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable();
   }

   private static Mock<IShufflingService> GeneratePlayerEnumerable()
   {
      var shufflingService = new Mock<IShufflingService>();
      var cards = new List<Card>();
      shufflingService
         .Setup(s => s.Shuffle(It.IsAny<IList<Card>>()))
         .Callback((IList<Card> a) => cards = a.ToList());
      return shufflingService;
   }
}