// 🃏 The HossGame 🃏
// <copyright file="StartNewShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

   #region

using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.GameAggregate.RoundEntity.Events;
using Hoss.Core.Interfaces;
using Moq;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;

#endregion

public sealed class StartNewShould
{
   [Theory]
   [AutoReadyGameData]
   public void HaveValidState([Frozen] AGame sut)
   {
      var teamPlayers = sut.FindGamePlayers().Select(g => new RoundPlayer(g.PlayerId, g.TeamId));
      sut.CurrentRoundState.Id.Should().NotBeNull();
      sut.CurrentRoundState.RoundPlayers.Should().Contain(teamPlayers);
      sut.CurrentRoundState.State.Should().Be(Round.RoundState.Bidding);
      sut.CurrentRoundState.PlayerDeals.Should().HaveCount(4);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseCardsDealtPerPlayer(AGame game)
   {
      var @event = game.Events.ShouldContain().ManyEventsOfType<PlayerCardsDealtEvent>(4);
      var events = @event.ToList();
      events.Should().NotBeNull();

      events.Should().HaveCount(4);
      events.Should().AllSatisfy(p => p.Cards.Cards.Should().HaveCount(6));
      events.Should().AllSatisfy(p => p.Should().BeAssignableTo<RoundEventBase>());

      var allCards = events.SelectMany(e => e.Cards.Cards).ToList();
      allCards.Should().HaveCount(24);
      allCards.GroupBy(c => c.Suit).Should().HaveCount(4);
      allCards.GroupBy(c => c.Rank).Should().HaveCount(6);
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseAllCardsDealtEvent([Frozen] AGame game)
   {
      var (gameId, roundId) = game.Events.ShouldContain().SingleEventOfType<AllCardsDealtEvent>();

      gameId.Should().Be(game.Id);
      roundId.Should().NotBeNull();

      game.CurrentRoundState.State.Should().Be(Round.RoundState.Bidding);
   }

   [Theory]
   [AutoReadyGameData]
   public void CallShuffleService([Frozen] Mock<IShufflingService> shuffleService, AGame sut)
   {
      sut.CurrentRoundState.PlayerDeals.Should().HaveCount(4);
      shuffleService.Verify(s => s.Shuffle(It.IsAny<IList<ACard>>()), Times.Once);
   }
}
