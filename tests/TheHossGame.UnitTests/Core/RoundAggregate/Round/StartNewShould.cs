// 🃏 The HossGame 🃏
// <copyright file="StartNewShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.RoundAggregate.Round;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate;
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
      var round = Round.StartNew(gameId, firstBidder);

      round.Should().NotBeNull();
      round.GameId.Should().Be(gameId);
      round.FirstBidder.Should().Be(firstBidder);
   }
}
