// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

using FluentAssertions;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.SharedKernel;
using TheHossGame.UnitTests.Core.Services;
using Xunit;

public class PlayerJoinedEventShould
{
   [Theory]
   [AutoMoqData]
   public void ShouldBeImmutable(PlayerJoinedEvent @event) =>
      @event.Should().BeAssignableTo<DomainEventBase>();
}