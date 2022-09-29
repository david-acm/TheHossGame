// 🃏 The HossGame 🃏
// <copyright file="StartNewShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.RoundAggregate.Round;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using Round = TheHossGame.Core.RoundAggregate.Round;

public class StartNewShould
{
   [Theory]
   [AutoPlayerData]
   public void StartNewRound(
      AGameId gameId,
      PlayerId firstBidder)
   {
      // Entity command parameter | Command method
      var round = Round.StartNew(gameId, firstBidder);

      // Entity state | Entity properties, When method
      round.Should().NotBeNull();
      round.GameId.Should().Be(gameId);
      round.FirstBidder.Should().Be(firstBidder);
      round.State.Should().Be(Round.RoundState.Started);

      // Entity events | Apply method
      round.Events.ShouldContain()
         .SingleEventOfType<RoundStartedEvent>();

      // Event content | Event
   }
}
