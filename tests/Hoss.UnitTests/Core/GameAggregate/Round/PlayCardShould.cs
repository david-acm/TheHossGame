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

        game.PlayCard(winner, new ACard(Jack, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Jack, Diamonds));

        game.PlayCard(game.CurrentPlayerId, new ACard(Ten, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Ace, Clubs));
        game.PlayCard(game.CurrentPlayerId, new ACard(Ten, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Ten, Diamonds));

        game.PlayCard(game.CurrentPlayerId, new ACard(Queen, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Jack, Clubs));
        game.PlayCard(game.CurrentPlayerId, new ACard(Jack, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Diamonds));

        game.PlayCard(game.CurrentPlayerId, new ACard(King, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Queen, Clubs));
        game.PlayCard(game.CurrentPlayerId, new ACard(Queen, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Queen, Diamonds));

        game.PlayCard(game.CurrentPlayerId, new ACard(Ace, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(King, Clubs));
        game.PlayCard(game.CurrentPlayerId, new ACard(King, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(King, Diamonds));

        game.PlayCard(game.CurrentPlayerId, new ACard(Ten, Clubs));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Clubs));
        game.PlayCard(game.CurrentPlayerId, new ACard(Ace, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Ace, Diamonds));

        game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(24);
        game.Events.ShouldContain().ManyEventsOfType<HandPlayedEvent>(6)
            .ToList()
            .ForEach(c => c.HandWinner.PlayerId.Should().Be(winner));
        game.CurrentPlayerId.Should().Be(winner);
    }

    [Theory]
    [BidFinishedGameData]
    public void CurrentPlayerShouldBeHandWinner(AGame game)
    {
        var winner = game.CurrentPlayerId;
        game.SelectTrump(winner, SuitWithMostCards(game, winner));
        game.PlayCard(winner, new ACard(Jack, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Hearts));
        game.PlayCard(game.CurrentPlayerId, new ACard(Nine, Spades));
        game.PlayCard(game.CurrentPlayerId, new ACard(Jack, Diamonds));

        game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(4);
        game.Events.ShouldContain().SingleEventOfType<HandPlayedEvent>()
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
    [AutoPlayerData]
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