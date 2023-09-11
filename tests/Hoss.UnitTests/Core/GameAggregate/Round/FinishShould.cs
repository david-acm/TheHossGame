// 🃏 The HossGame 🃏
// <copyright file="FinishShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
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
        @event.GameId.Should().Be(game.Id);
    }
}