// 🃏 The HossGame 🃏
// <copyright file="FinishShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using FluentAssertions;
using TheHossGame.UnitTests.Extensions;

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

#region

using Hoss.Core.GameAggregate;
using PlayerAggregate.Generators;
using Xunit;

#endregion

public class FinishShould
{
    [Theory]
    [ReadyGameData]
    public void RaiseGameFinishEvent(AGame game)
    {
        game.Finish();

        var @event = game.Events.ShouldContain().SingleEventOfType<GameEvents.GameFinishedEvent>();

        @event.Should().BeAssignableTo<GameEvents.GameEventBase>();
        @event.GameId.Id.Should().Be(game.Id);
    }
}