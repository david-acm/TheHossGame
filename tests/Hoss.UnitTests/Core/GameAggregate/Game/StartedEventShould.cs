// 🃏 The HossGame 🃏
// <copyright file="StartedEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;
using Services;
using Xunit;

#endregion

public class StartedEventShould
{
    [Theory]
    [AutoMoqData]
    public void ShouldBeImmutable(GameEvents.NewGameCreatedEvent @event)
    {
        @event.Should().BeAssignableTo<DomainEventBase>();
    }
}