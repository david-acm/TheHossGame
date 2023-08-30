// 🃏 The HossGame 🃏
// <copyright file="PlayCardShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects.Rank;
using static Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects.Suit;

public class PlayCardShould
{
    [Theory]
    [BidFinishedGameData]
    public void StartNewHandWhenAHandIsPlayed(AGame game)
    {
        var winner = game.CurrentPlayerId;
        game.SelectTrump(winner, SuitWithMostCards(game, winner));
        // Hand #1
        game.PlayerInTurnPlays(new(Jack, Hearts));
        var nextRoundFirstPlayer = game.CurrentPlayerId;
        game.PlayerInTurnPlays(new(Nine, Hearts));
        game.PlayerInTurnPlays(new(Nine, Spades));
        game.PlayerInTurnPlays(new(Jack, Diamonds));
        // Hand #2
        game.PlayerInTurnPlays(new(Ten, Hearts));
        game.PlayerInTurnPlays(new(Ace, Clubs));
        game.PlayerInTurnPlays(new(Ten, Spades));
        game.PlayerInTurnPlays(new(Ten, Diamonds));
        // Hand #3
        game.PlayerInTurnPlays(new(Queen, Hearts));
        game.PlayerInTurnPlays(new(Jack, Clubs));
        game.PlayerInTurnPlays(new(Jack, Spades));
        game.PlayerInTurnPlays(new(Nine, Diamonds));
        // Hand #4
        game.PlayerInTurnPlays(new(King, Hearts));
        game.PlayerInTurnPlays(new(Queen, Clubs));
        game.PlayerInTurnPlays(new(Queen, Spades));
        game.PlayerInTurnPlays(new(Queen, Diamonds));
        // Hand #5
        game.PlayerInTurnPlays(new(Ace, Hearts));
        game.PlayerInTurnPlays(new(King, Clubs));
        game.PlayerInTurnPlays(new(King, Spades));
        game.PlayerInTurnPlays(new(King, Diamonds));
        //Hand #6
        game.PlayerInTurnPlays(new(Ten, Clubs));
        game.PlayerInTurnPlays(new(Nine, Clubs));
        game.PlayerInTurnPlays(new(Ace, Spades));
        game.PlayerInTurnPlays(new(Ace, Diamonds));

        // Assert
        game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(24);
        game.Events.ShouldContain().ManyEventsOfType<TrickPlayedEvent>(6)
            .ToList()
            .ForEach(c => c.HandWinner.PlayerId.Should().Be(winner));
        var roundPlayed = game.Events.ShouldContain().SingleEventOfType<RoundPlayedEvent>();
        roundPlayed.Score.team1.Score.Should().Be(6);
        roundPlayed.Score.team2.Score.Should().Be(0);

        game.CurrentPlayerId.Should().Be(nextRoundFirstPlayer);
    }

    [Theory]
    [BidFinishedGameData]
    public void NotAllowACardThatDoesNotFollowSuitWhenALeftBarIsAvailable(AGame game)
    {
        var winner = game.CurrentPlayerId;
        game.SelectTrump(winner, SuitWithMostCards(game, winner));

        game.PlayerInTurnPlays(new(Jack, Hearts));
        game.PlayerInTurnPlays(new(Nine, Hearts));
        game.PlayerInTurnPlays(new(Nine, Spades));
        var forbiddenPlay = () => game.PlayerInTurnPlays(new(Ace, Diamonds));

        forbiddenPlay.Should().Throw<InvalidEntityStateException>();
        game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(3);
        game.Events.ShouldContain().NoEventsOfType<TrickPlayedEvent>();
    }

    [Theory]
    [BidFinishedGameData]
    public void MakeWinnerTheNextPlayer(AGame game)
    {
        var winner = game.CurrentPlayerId;
        game.SelectTrump(winner, SuitWithMostCards(game, winner));
        game.PlayCard(winner, new ACard(Jack, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Jack, Diamonds));

        game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(4);
        game.Events.ShouldContain().SingleEventOfType<TrickPlayedEvent>()
            .HandWinner.Should().Be(new CardPlay(winner, new ACard(Jack, Hearts)));
        game.CurrentPlayerId.Should().Be(winner);
    }

    [Theory]
    [BidFinishedGameData]
    public void RaiseEventWhenRoundActive(AGame game)
    {
        var currentPlayerId = game.CurrentPlayerId;
        game.SelectTrump(currentPlayerId, Hearts);
        var playedCard = game.CurrentRoundState.PlayerDeals.First(p => p.PlayerId == currentPlayerId).Cards[0];
        game.PlayCard(currentPlayerId, playedCard);

        game.Events.ShouldContain().SingleEventOfType<CardPlayedEvent>();
        game.CurrentRoundState.TableCenter.Should().Contain(cp => cp.Card == playedCard);
        game.CurrentRoundState.DealForPlayer(currentPlayerId).Cards.Should().NotContain(playedCard);
    }

    [Theory]
    [HossRoundData]
    public void AllowDifferentSuitWhenNotAvailable(AGame game)
    {
        var currentPlayerId = game.CurrentPlayerId;
        game.SelectTrump(currentPlayerId, SuitWithMostCards(game, currentPlayerId));

        var leadCard = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards[0];

        game.PlayCard(currentPlayerId, leadCard);

        var wrongSuitCard = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards[0];

        game.PlayCard(game.CurrentPlayerId, wrongSuitCard);

        game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(2);
    }

    [Theory]
    [BidFinishedGameData]
    public void ThrowEntityInvalidExceptionWhenCardDoesNotFollowSuit(AGame game)
    {
        var currentPlayerId = game.CurrentPlayerId;
        game.SelectTrump(currentPlayerId, SuitWithMostCards(game, currentPlayerId));
        var leadCard = game.CurrentRoundState.PlayerDeals.First(p => p.PlayerId == currentPlayerId).Cards[0];
        game.PlayCard(currentPlayerId, leadCard);

        var wrongSuitCard = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards
            .First(c => c.Suit != leadCard.Suit);
        var action = () => game.PlayCard(game.CurrentPlayerId, wrongSuitCard);

        game.Events.ShouldContain().SingleEventOfType<CardPlayedEvent>();
        action.Should().Throw<InvalidEntityStateException>();
    }

    [Theory]
    [BidFinishedGameData]
    public void ThrowExceptionWhenCardNotInPlayerDeal(AGame game)
    {
        game.SelectTrump(game.CurrentPlayerId, Hearts);
        var card = game.CurrentRoundState.PlayerDeals.First(pd => pd != game.CurrentPlayerId)!.Cards[0];
        var action = () => game.PlayCard(game.CurrentPlayerId, card);

        action.Should().Throw<InvalidEntityStateException>();

        game.Events.ShouldContain().NoEventsOfType<CardPlayedEvent>();
    }

    [Theory]
    [PlayerData]
    public void NotRaiseEventWhenNoRoundActive(AGame game)
    {
        game.PlayCard(game.CurrentPlayerId, new ACard(King, Clubs));

        game.Events.ShouldContain().NoEventsOfType<CardPlayedEvent>();
    }

    [Theory]
    [BidFinishedGameData]
    public void ThrowExceptionWhenPlayerNotInTurn(AGame game)
    {
        game.SelectTrump(game.CurrentPlayerId, Hearts);
        var playerNotInTurn = game.FindGamePlayers().FirstOrDefault(p => p.Id != game.CurrentPlayerId);

        var playAction = () => game.PlayCard(playerNotInTurn!.PlayerId, new ACard(King, Clubs));

        playAction.Should().Throw<InvalidEntityStateException>();
    }

    private static Suit SuitWithMostCards(AGame game, PlayerId currentPlayerId)
    {
        return game.CurrentRoundState.DealForPlayer(currentPlayerId!).Cards.GroupBy(c => c.Suit)
            .OrderByDescending(s => s.Count()).First().Key;
    }
}

public static class GameExtensions
{
    internal static void PlayerInTurnPlays(this AGame game, ACard aCard)
    {
        game.PlayCard(game.CurrentPlayerId, aCard);
    }
}