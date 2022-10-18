// 🃏 The HossGame 🃏
// <copyright file="TeamsFormedEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.SharedKernel;
using TheHossGame.UnitTests.Core.Services;
using Xunit;

public class TeamsFormedEventShould
{
   [Theory]
   [AutoMoqData]
   public void ShouldBeImmutable(TeamsFormedEvent @event)
   {
      @event.Should().BeAssignableTo<DomainEventBase>();
   }
}
