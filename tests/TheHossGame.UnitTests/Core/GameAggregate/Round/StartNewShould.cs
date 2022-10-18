// 🃏 The HossGame 🃏
// <copyright file="StartNewShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.RoundAggregate.Round;

using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.Interfaces;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;

public sealed class StartNewShould
{
   [Theory]
   [AutoReadyGameData]
   public void HaveValidState([Frozen] AGame sut)
   {
      var teamPlayers = sut.FindTeamPlayers().Select(g => new RoundPlayer(g.PlayerId, g.TeamId));
      sut.CurrentRound.Id.Should().NotBeNull();
      sut.CurrentRound.TeamPlayers.Should().Contain(teamPlayers);
      sut.CurrentRound.State.Should().Be(Round.RoundState.CardsDealt);
      sut.CurrentRound.PlayerDeals.Should().HaveCount(4);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseCardsDealtPerPlayer(AGame game)
   {
      var @event = game.Events.ShouldContain()
         .ManyEventsOfType<PlayerCardsDealtEvent>(4);
      var events = @event.ToList();
      events.Should().NotBeNull();

      events.Should().HaveCount(4);
      events.Should().AllSatisfy(
         p => p.PlayerCards.Cards.Should().HaveCount(6));

      var allCards = events.SelectMany(e => e.PlayerCards.Cards).ToList();
      allCards.Should().HaveCount(24);
      allCards.GroupBy(c => c.Suit).Should().HaveCount(4);
      allCards.GroupBy(c => c.Rank).Should().HaveCount(6);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseAllCardsDealtEvent([Frozen] AGame game)
   {
      (GameId? gameId, RoundId? roundId) = game.Events.ShouldContain()
         .SingleEventOfType<AllCardsDealtEvent>();

      gameId.Should().Be(game.Id);
      roundId.Should().NotBeNull();

      game.CurrentRound.State.Should().Be(Round.RoundState.CardsDealt);
   }

   [Theory]
   [AutoReadyGameData]
   public void CallShuffleService(
      [Frozen] Mock<IShufflingService> shuffleService,
      AGame sut)
   {
      sut.CurrentRound.PlayerDeals.Should().HaveCount(4);
      shuffleService.Verify(
      s => s.Shuffle(It.IsAny<IList<ACard>>()),
      Times.Once);
   }
}
