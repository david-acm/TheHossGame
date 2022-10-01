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
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using Round = TheHossGame.Core.RoundAggregate.Round;

public class StartNewShould
{
   [Theory]
   [AutoReadyGameData]
   public void HaveValidState(
      [Frozen] AGameId gameId,
      [Frozen] IEnumerable<Player> roundPlayers,
      Round sut)
   {
      var roundPLayerIds = roundPlayers.Select(p => p.Id);

      sut.Should().NotBeNull();
      sut.Id.Should().NotBeNull();
      sut.GameId.Should().Be(gameId);
      sut.RoundPlayers.Should().Contain(roundPLayerIds);
      sut.State.Should().Be(Round.RoundState.Started);
      sut.PlayerCards.Should().HaveCount(4);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseStartEvent(
      [Frozen] AGameId gameId,
      Round sut)
   {
      var startedEvent = sut.Events.ShouldContain()
      .SingleEventOfType<RoundStartedEvent>();

      startedEvent.GameId.Should().Be(gameId);
      startedEvent.RoundId.Should().NotBeNull();
      startedEvent.Deck.Should().NotBeNull();
   }

   [Theory]
   [AutoReadyGameData]
   public void DealCards(
   Round sut)
   {
      var startedEvent = sut.Events.ShouldContain()
         .SingleEventOfType<RoundStartedEvent>();
      startedEvent.Deck.Cards.Should().BeEmpty();
      startedEvent.PlayerCards.Should().HaveCount(4);
      startedEvent.PlayerCards
         .Should().AllSatisfy(p => p.Cards.Should().HaveCount(6));

      var allCards = startedEvent.PlayerCards.SelectMany(p => p.Cards);
      allCards.Should().HaveCount(24);
      allCards.GroupBy(c => c.Suit).Should().HaveCount(4);
      allCards.GroupBy(c => c.Rank).Should().HaveCount(6);
   }

   [Theory]
   [AutoReadyGameData]
   public void CallShuffleService(
   [Frozen] Mock<IShufflingService> shuffleService,
   Round sut)
   {
      sut.PlayerCards.Should().HaveCount(4);
      shuffleService.Verify(
      s => s.Shuffle(It.IsAny<IList<Card>>()),
      Times.Once);
   }
}
