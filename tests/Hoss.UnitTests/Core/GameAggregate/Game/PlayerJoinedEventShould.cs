// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Services;
using Xunit;

#endregion

public class PlayerJoinedEventShould
{
    [Theory]
    [AutoMoqData]
    public void ShouldBeImmutable(GameEvents.PlayerJoinedEvent @event)
    {
        @event.Should().BeAssignableTo<GameEvents.GameEventBase>();
    }
}