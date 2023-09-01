// 🃏 The HossGame 🃏
// <copyright file="PlayCardShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
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
        roundPlayed.RoundScore.team1.Score.Should().Be(6);
        roundPlayed.RoundScore.team2.Score.Should().Be(0);

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
    [BidWithHossGameData]
    public void EndGameWhenGameReachesMaximumScore(AGame game)
    {
        var winner = game.CurrentPlayerId;
        // First round is Hoss
        game.SelectTrump(winner, SuitWithMostCards(game, winner));
        // First hand ensures Player will fish the other trump
        game.PlayerInTurnPlays(new(Jack, Hearts));
        game.PlayerInTurnPlaysAValidCard();
        game.PlayerInTurnPlaysAValidCard();

        // Rest of the hands for the first round
        for (var hand = 2; hand <= 6; hand++)
        {
            game.PlayerInTurnPlaysAValidCard();
            game.PlayerInTurnPlaysAValidCard();
            game.PlayerInTurnPlaysAValidCard();
        }

        var roundPlayed = game.Events.ShouldContain()
            .SingleEventOfType<RoundPlayedEvent>();
        roundPlayed.RoundScore.team1.Score.Should().Be(12);
        roundPlayed.RoundScore.team2.Score.Should().Be(0);

        for (var player = 1; player <= 4; player++)
            game.Bid(game.CurrentPlayerId, game.CurrentPlayerId == winner ? BidValue.Six : BidValue.Pass);

        game.SelectTrump(winner, SuitWithMostCards(game, winner));

        // Round playing
        for (var round = 2; round <= 6; round++)
        {
            // Hand playing
            for (var hand = 1; hand <= 6; hand++)
            {
                game.PlayerInTurnPlaysAValidCard();
                game.PlayerInTurnPlaysAValidCard();
                game.PlayerInTurnPlaysAValidCard();
                game.PlayerInTurnPlaysAValidCard();
            }

            const int playedCardsInFirstRound = 18;

            int GetPlayedCardCount()
            {
                return 24 * (round - 1);
            }

            game.Events.ShouldContain()
                .ManyEventsOfType<CardPlayedEvent>(playedCardsInFirstRound + GetPlayedCardCount());
            game.Events.ShouldContain().ManyEventsOfType<TrickPlayedEvent>(6 * round);

            roundPlayed = game.Events.ShouldContain()
                .ManyEventsOfType<RoundPlayedEvent>(round).Last();
            roundPlayed.RoundScore.team1.Score.Should().Be(6);
            roundPlayed.RoundScore.team2.Score.Should().Be(0);

            if (game.Stage == AGame.GameState.Finished)
                break;
            // Bidding and Trump Selection
            for (var player = 1; player <= 4; player++)
                game.Bid(game.CurrentPlayerId, game.CurrentPlayerId == winner ? BidValue.Six : BidValue.Pass);

            game.SelectTrump(winner, SuitWithMostCards(game, winner));
        }

        game.Events.ShouldContain().SingleEventOfType<GameFinishedEvent>();
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
    public void NotAllowToPlayACardThatDoesNotFollowSuit(AGame game)
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
    public void NotAllowToPlayACardNotPresentInThePlayersDeal(AGame game)
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
    public void NotAllowAPlayerNotInTurnToPlay(AGame game)
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

    internal static void PlayerInTurnPlaysAValidCard(this AGame game)
    {
        var currentPlayerCards = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards;
        var lastPlayedCard = game.CurrentRoundState.TableCenter.LastOrDefault()?.Card;
        var comparer = new Card.CardComparer(game.CurrentRoundState.TrumpSelected, lastPlayedCard?.Suit);
        var sameSuitCard = currentPlayerCards.OrderByDescending(c => c, comparer).First();
        game.PlayCard(game.CurrentPlayerId, sameSuitCard);
    }
}