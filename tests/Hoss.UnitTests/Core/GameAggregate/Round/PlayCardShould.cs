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
using static Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects.Suit;

public class PlayCardShould
{
   [Theory]
   [AutoBidFinishedGameData]
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
      game.SelectTrump(currentPlayerId, this.SuitWithMostCards(game, currentPlayerId));

      var leadCard = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards[0];

      game.PlayCard(currentPlayerId, leadCard);

      var wrongSuitCard = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards[0];

      game.PlayCard(game.CurrentPlayerId, wrongSuitCard);

      game.Events.ShouldContain().ManyEventsOfType<CardPlayedEvent>(2);
   }

   private Suit SuitWithMostCards(AGame game, PlayerId currentPlayerId) => game.CurrentRoundState.DealForPlayer(currentPlayerId!).Cards.GroupBy(c => c.Suit).OrderByDescending(s => s.Count()).First().Key;

   [Theory]
   [AutoBidFinishedGameData]
   public void ThrowEntityInvalidExceptionWhenCardDoesNotFollowSuit(AGame game)
   {
      var currentPlayerId = game.CurrentPlayerId;
      game.SelectTrump(currentPlayerId, this.SuitWithMostCards(game, currentPlayerId));
      var leadCard = game.CurrentRoundState.PlayerDeals.First(p => p.PlayerId == currentPlayerId).Cards[0];
      game.PlayCard(currentPlayerId, leadCard);

      var wrongSuitCard = game.CurrentRoundState.DealForPlayer(game.CurrentPlayerId).Cards.First(c => c.Suit != leadCard.Suit);
      var action = () => game.PlayCard(game.CurrentPlayerId, wrongSuitCard);

      game.Events.ShouldContain().SingleEventOfType<CardPlayedEvent>();
      action.Should().Throw<InvalidEntityStateException>();
   }

   [Theory]
   [AutoBidFinishedGameData]
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
      game.PlayCard(game.CurrentPlayerId, new ACard(Clubs, Rank.King));

      game.Events.ShouldContain().NoEventsOfType<CardPlayedEvent>();
   }

   [Theory]
   [AutoBidFinishedGameData]
   public void ThrowExceptionWhenPlayerNotInTurn(AGame game)
   {
      game.SelectTrump(game.CurrentPlayerId, Hearts);
      var playerNotInTurn = game.FindGamePlayers().FirstOrDefault(p => p.Id != game.CurrentPlayerId);

      var playAction = () => game.PlayCard(playerNotInTurn!.PlayerId, new ACard(Clubs, Rank.King));

      playAction.Should().Throw<InvalidEntityStateException>();
   }
}
