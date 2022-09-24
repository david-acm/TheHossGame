// 🃏 The HossGame 🃏
// <copyright file="StartedEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.UnitTests.Core.Services;
using Xunit;

public class StartedEventShould
{
   [Theory]
   [AutoMoqData]
   public void ShouldBeImmutable(GameStartedEvent @event) =>
      @event.Should().
      BeAssignableTo<DomainEventBase>();
}
