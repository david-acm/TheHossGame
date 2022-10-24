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
using Hoss.Core.GameAggregate.Events;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;

#endregion

public class FinishShould
{
   [Theory]
   [AutoReadyGameData]
   public void RaiseGameFinishEvent(AGame game)
   {
      game.Finish();

      var @event = game.Events.ShouldContain().SingleEventOfType<GameFinishedEvent>();

      @event.Should().BeAssignableTo<GameEventBase>();
      @event.GameId.Should().Be(game.Id);
   }
}
