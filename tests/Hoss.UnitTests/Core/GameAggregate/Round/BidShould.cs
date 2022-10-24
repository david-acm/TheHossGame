// 🃏 The HossGame 🃏
// <copyright file="BidShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.Events;
using Hoss.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static Hoss.Core.GameAggregate.Game.TeamId;

#endregion

public class BidShould
{
   [Theory]
   [AutoReadyGameData]
   public void RaiseBidCompletedEvent(AGame game)
   {
      game.Bid(game.CurrentPlayerId, BidValue.One);
      game.Bid(game.CurrentPlayerId, BidValue.Two);
      var winnerPlayerId = game.CurrentPlayerId;
      game.Bid(winnerPlayerId, BidValue.Four);
      game.Bid(game.CurrentPlayerId, BidValue.Pass);

      var (gameId, roundId, winningBid) = game.Events.ShouldContain().SingleEventOfType<BidCompleteEvent>();

      gameId.Should().Be(game.Id);
      roundId.Should().Be(game.CurrentRoundState.Id);
      winningBid.Value.Should().Be(BidValue.Four);
      winningBid.PlayerId.Should().Be(winnerPlayerId);
      game.CurrentPlayerId.Should().Be(winnerPlayerId);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseBidEvent(AGame game, BidValue value)
   {
      var bidPlayerId = game.CurrentPlayerId;
      game.Bid(bidPlayerId, value);
      var (gameId, roundId, bid) = game.Events.ShouldContain().SingleEventOfType<BidEvent>();

      gameId.Should().Be(game.Id);
      roundId.Should().Be(game.CurrentRoundState.Id);
      bid.PlayerId.Should().Be(bidPlayerId);
      bid.Value.Should().Be(value);
   }

   [Theory]
   [AutoPlayerData]
   public void NotRaiseBidEvent(AGame game, BidValue value)
   {
      var bidPlayerId = game.CurrentPlayerId;
      game.Bid(bidPlayerId, value);
      game.Events.ShouldContain().NoEventsOfType<BidEvent>();
   }

   [Theory]
   [AutoReadyGameData]
   public void AddBidToRound(AGame game, BidValue value)
   {
      var bidPlayerId = game.CurrentPlayerId;
      game.Bid(bidPlayerId, value);
      game.CurrentRoundState.State.Should().Be(Round.RoundState.CardsDealt);
      var bid = game.CurrentRoundState.Bids.Should().ContainSingle().Subject;
      bid.PlayerId.Should().Be(bidPlayerId);
      bid.Value.Should().Be(value);
   }

   [Theory]
   [AutoReadyGameData]
   public void AllowPlayersToPass(AGame game)
   {
      game.Bid(game.CurrentPlayerId, BidValue.One);
      game.Bid(game.CurrentPlayerId, BidValue.Two);
      game.Bid(game.CurrentPlayerId, BidValue.Pass);

      game.CurrentRoundState.Bids.Should().HaveCount(3);
   }

   [Theory]
   [AutoReadyGameData]
   public void ThrowInvalidEntityExceptionWhenBidLowerThanOthers(AGame game)
   {
      game.Bid(game.CurrentPlayerId, BidValue.One);
      game.Bid(game.CurrentPlayerId, BidValue.Two);

      var bidAction = () => game.Bid(game.CurrentPlayerId, BidValue.One);

      bidAction.Should().Throw<InvalidEntityStateException>();
   }

   [Theory]
   [AutoReadyGameData]
   public void PlayerOrderShouldBeCorrect(AGame game)
   {
      game.CurrentRoundState.RoundPlayers.Should().ContainInOrder(game.CurrentRoundState.RoundPlayers.First(p => p.TeamId == Team1), game.CurrentRoundState.RoundPlayers.First(p => p.TeamId == Team2), game.CurrentRoundState.RoundPlayers.Last(p => p.TeamId == Team1), game.CurrentRoundState.RoundPlayers.Last(p => p.TeamId == Team2));
   }

   [Theory]
   [AutoReadyGameData]
   public void ThrowInvalidEntityExceptionWhenOutOfTurn(AGame game)
   {
      game.Bid(game.CurrentPlayerId, BidValue.One);

      var bidAction = () => game.Bid(game.CurrentPlayerId, BidValue.One);

      bidAction.Should().Throw<InvalidEntityStateException>();
   }

   [Theory]
   [AutoReadyGameData]
   public void ThrowInvalidEntityExceptionWhenPLayerNotInGame(APlayerId playerId, AGame game)
   {
      var bidAction = () => game.Bid(playerId, BidValue.One);

      bidAction.Should().Throw<InvalidEntityStateException>();
   }
}
