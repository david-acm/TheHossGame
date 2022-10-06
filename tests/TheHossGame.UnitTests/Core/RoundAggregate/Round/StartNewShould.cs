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
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using Round = TheHossGame.Core.RoundAggregate.Round;

public sealed class StartNewShould
{
   [Theory]
   [AutoReadyGameData]
   public void HaveValidState(
      [Frozen] AGame game,
      Round sut)
   {
      var teamPlayers = game.FindTeamPlayers().Select(g => new TeamPlayer(g.PlayerId, g.TeamId));
      sut.Id.Should().NotBeNull();
      sut.GameId.Should().Be(game.Id);
      sut.TeamPlayers.Should().Contain(teamPlayers);
      sut.State.Should().Be(Round.RoundState.CardsDealt);
      sut.PlayerDeals.Should().HaveCount(4);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseStartEvent(
      [Frozen] AGame game,
      Round sut)
   {
      var startedEvent = sut.Events.ShouldContain()
         .SingleEventOfType<RoundStartedEvent>();

      startedEvent.GameId.Should().Be(game.Id);
      startedEvent.RoundId.Should().NotBeNull();
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseCardsDealtPerPlayer(Round sut)
   {
      var @event = sut.Events.ShouldContain()
         .ManyEventsOfType<PlayerCardsDealtEvent>(4);
      @event.Should().NotBeNull();

      @event.Should().HaveCount(4);
      @event.Should().AllSatisfy(
         p => p.playerCards.Cards.Should().HaveCount(6));

      var allCards = @event.SelectMany(e => e.playerCards.Cards);
      allCards.Should().HaveCount(24);
      allCards.GroupBy(c => c.Suit).Should().HaveCount(4);
      allCards.GroupBy(c => c.Rank).Should().HaveCount(6);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseAllCardsDealtEvent(
      [Frozen] AGame game,
      Round sut)
   {
      var startedEvent = sut.Events.ShouldContain()
         .SingleEventOfType<AllCardsDealtEvent>();

      startedEvent.GameId.Should().Be(game.Id);
      startedEvent.RoundId.Should().NotBeNull();

      sut.State.Should().Be(Round.RoundState.CardsDealt);
   }

   [Theory]
   [AutoReadyGameData]
   public void CallShuffleService(
      [Frozen] Mock<IShufflingService> shuffleService,
      Round sut)
   {
      sut.PlayerDeals.Should().HaveCount(4);
      shuffleService.Verify(
      s => s.Shuffle(It.IsAny<IList<Card>>()),
      Times.Once);
   }
}
