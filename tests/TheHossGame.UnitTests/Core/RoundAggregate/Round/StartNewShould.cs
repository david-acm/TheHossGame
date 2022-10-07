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
using ARound = TheHossGame.Core.RoundAggregate.ARound.RoundState;

public sealed class StartNewShould
{
   [Theory]
   [AutoReadyGameData]
   public void HaveValidState([Frozen] AGame sut)
   {
      var teamPlayers = sut.FindTeamPlayers().Select(g => new TeamPlayer(g.PlayerId, g.TeamId));
      sut.CurrentRound.Id.Should().NotBeNull();
      sut.CurrentRound.TeamPlayers.Should().Contain(teamPlayers);
      sut.CurrentRound.State.Should().Be(ARound.CardsDealt);
      sut.CurrentRound.PlayerDeals.Should().HaveCount(4);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseCardsDealtPerPlayer(AGame game)
   {
      var @event = game.Events.ShouldContain()
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
   public void RaiseAllCardsDealtEvent([Frozen] AGame game)
   {
      var startedEvent = game.Events.ShouldContain()
         .SingleEventOfType<AllCardsDealtEvent>();

      startedEvent.GameId.Should().Be(game.Id);
      startedEvent.RoundId.Should().NotBeNull();

      game.CurrentRound.State.Should().Be(ARound.CardsDealt);
   }

   [Theory]
   [AutoReadyGameData]
   public void CallShuffleService(
      [Frozen] Mock<IShufflingService> shuffleService,
      AGame sut)
   {
      sut.CurrentRound.PlayerDeals.Should().HaveCount(4);
      shuffleService.Verify(
      s => s.Shuffle(It.IsAny<IList<Card>>()),
      Times.Once);
   }
}
