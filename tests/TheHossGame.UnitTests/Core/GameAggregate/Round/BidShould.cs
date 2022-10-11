// 🃏 The HossGame 🃏
// <copyright file="BidShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static TheHossGame.Core.GameAggregate.Game.TeamId;

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

      var @event = game.Events.ShouldContain()
         .SingleEventOfType<BidCompleteEvent>();

      @event.GameId.Should().Be(game.Id);
      @event.RoundId.Should().Be(game.CurrentRound.Id);
      @event.WinningBid.Value.Should().Be(BidValue.Four);
      @event.WinningBid.PlayerId.Should().Be(winnerPlayerId);
      game.CurrentPlayerId.Should().Be(winnerPlayerId);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseBidEvent(AGame game, BidValue value)
   {
      var bidPlayerId = game.CurrentPlayerId;
      game.Bid(bidPlayerId, value);
      var @event = game.Events.ShouldContain()
         .SingleEventOfType<BidEvent>();

      @event.GameId.Should().Be(game.Id);
      @event.RoundId.Should().Be(game.CurrentRound.Id);
      @event.Bid.PlayerId.Should().Be(bidPlayerId);
      @event.Bid.Value.Should().Be(value);
   }

   [Theory]
   [AutoReadyGameData]
   public void AddBidToRound(AGame game, BidValue value)
   {
      var bidPlayerId = game.CurrentPlayerId;
      game.Bid(bidPlayerId, value);
      game.CurrentRound.State.Should().Be(Round.RoundState.CardsDealt);
      var bid = game.CurrentRound.Bids.Should().ContainSingle().Subject;
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

      game.CurrentRound.Bids.Should().HaveCount(3);
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
      game.CurrentRound.TeamPlayers.Should().ContainInOrder(
         game.CurrentRound.TeamPlayers.First(p => p.TeamId == Team1),
         game.CurrentRound.TeamPlayers.First(p => p.TeamId == Team2),
         game.CurrentRound.TeamPlayers.Last(p => p.TeamId == Team1),
         game.CurrentRound.TeamPlayers.Last(p => p.TeamId == Team2));
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
   public void ThrowInvalidEntityExceptionWhenPLayerNotInGame(
      PlayerId playerId,
      AGame game)
   {
      var bidAction = () => game.Bid(playerId, BidValue.One);

      bidAction.Should().Throw<InvalidEntityStateException>();
   }
}
