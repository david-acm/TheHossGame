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
    [ReadyGameData]
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
    [ReadyGameData]
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
    [PlayerData]
    public void NotRaiseBidEventWhenRoundHasNotStarted(AGame game, BidValue value)
    {
        var bidPlayerId = game.CurrentPlayerId;
        game.Bid(bidPlayerId, value);
        game.Events.ShouldContain().NoEventsOfType<BidEvent>();
    }

    [Theory]
    [BidFinishedGameData]
    public void NotAllowBidsOutOfBiddingStage(AGame game, BidValue value)
    {
        var winner = game.CurrentPlayerId;
        var forbiddenPlay = () => game.Bid(game.CurrentPlayerId, value);

        forbiddenPlay.Should().Throw<InvalidEntityStateException>();
        game.Events.ShouldContain().ManyEventsOfType<BidEvent>(4);
    }

    [Theory]
    [ReadyGameData]
    public void AddBidToRound(AGame game, BidValue value)
    {
        var bidPlayerId = game.CurrentPlayerId;
        game.Bid(bidPlayerId, value);
        game.CurrentRoundState.State.Should().Be(Round.RoundState.Bidding);
        var bid = game.CurrentRoundState.Bids.Should().ContainSingle().Subject;
        bid.PlayerId.Should().Be(bidPlayerId);
        bid.Value.Should().Be(value);
    }

    [Theory]
    [ReadyGameData]
    public void AllowPlayersToPass(AGame game)
    {
        game.Bid(game.CurrentPlayerId, BidValue.One);
        game.Bid(game.CurrentPlayerId, BidValue.Two);
        game.Bid(game.CurrentPlayerId, BidValue.Pass);

        game.CurrentRoundState.Bids.Should().HaveCount(3);
    }

    [Theory]
    [ReadyGameData]
    public void ThrowInvalidEntityExceptionWhenBidLowerThanOthers(AGame game)
    {
        game.Bid(game.CurrentPlayerId, BidValue.One);
        game.Bid(game.CurrentPlayerId, BidValue.Two);

        var bidAction = () => game.Bid(game.CurrentPlayerId, BidValue.One);

        bidAction.Should().Throw<InvalidEntityStateException>();
    }

    [Theory]
    [ReadyGameData]
    public void PlayerOrderShouldBeCorrect(AGame game)
    {
        game.CurrentRoundState.RoundPlayers.Should().ContainInOrder(
            game.CurrentRoundState.RoundPlayers.First(p => p.TeamId == Team1),
            game.CurrentRoundState.RoundPlayers.First(p => p.TeamId == Team2),
            game.CurrentRoundState.RoundPlayers.Last(p => p.TeamId == Team1),
            game.CurrentRoundState.RoundPlayers.Last(p => p.TeamId == Team2));
    }

    [Theory]
    [ReadyGameData]
    public void ThrowInvalidEntityExceptionWhenOutOfTurn(AGame game)
    {
        game.Bid(game.CurrentPlayerId, BidValue.One);

        var bidAction = () => game.Bid(game.CurrentPlayerId, BidValue.One);

        bidAction.Should().Throw<InvalidEntityStateException>();
    }

    [Theory]
    [ReadyGameData]
    public void ThrowInvalidEntityExceptionWhenPLayerNotInGame(APlayerId playerId, AGame game)
    {
        var bidAction = () => game.Bid(playerId, BidValue.One);

        bidAction.Should().Throw<InvalidEntityStateException>();
    }
}