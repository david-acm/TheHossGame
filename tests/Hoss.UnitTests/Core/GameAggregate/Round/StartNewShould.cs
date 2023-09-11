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
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.Interfaces;
using Moq;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static Hoss.Core.GameAggregate.RoundEntity.RoundEvents;

#endregion

public sealed class StartNewShould
{
    [Theory]
    [ReadyGameData]
    public void HaveValidState([Frozen] AGame sut)
    {
        var teamPlayers = sut.FindGamePlayers().Select(g => new RoundPlayer(g.PlayerId, g.TeamId));
        sut.CurrentRoundView.Id.Should().NotBeNull();
        sut.CurrentRoundView.RoundPlayers.Should().Contain(teamPlayers);
        sut.CurrentRoundView.Stage.Should().Be(RoundBase.RoundStage.Bidding);
        sut.CurrentRoundView.PlayerDeals.Should().HaveCount(4);
    }

    [Theory]
    [ReadyGameData]
    public void RaiseCardsDealtPerPlayer(AGame game)
    {
        var @event = game.Events.ShouldContain().ManyEventsOfType<PlayerCardsDealtEvent>(4);
        var events = @event.ToList();
        events.Should().NotBeNull();

        events.Should().HaveCount(4);
        events.Should().AllSatisfy(p => p.Deal.Cards.Should().HaveCount(6));
        events.Should().AllSatisfy(p => p.Should().BeAssignableTo<RoundEventBase>());

        var allCards = events.SelectMany(e => e.Deal.Cards).ToList();
        allCards.Should().HaveCount(24);
        allCards.GroupBy(c => c.Suit).Should().HaveCount(4);
        allCards.GroupBy(c => c.Rank).Should().HaveCount(6);
    }

    [Theory]
    [ReadyGameData]
    public void RaiseAllCardsDealtEvent([Frozen] AGame game)
    {
        var (gameId, roundId) = game.Events.ShouldContain().SingleEventOfType<AllCardsDealtEvent>();

        gameId.Should().Be(game.Id);
        roundId.Should().NotBeNull();

        game.CurrentRoundView.Stage.Should().Be(RoundBase.RoundStage.Bidding);
    }

    [Theory]
    [ReadyGameData]
    public void CallShuffleService([Frozen] Mock<IShufflingService> shuffleService, AGame sut)
    {
        sut.CurrentRoundView.PlayerDeals.Should().HaveCount(4);
        shuffleService.Verify(s => s.Shuffle(It.IsAny<IList<ACard>>()), Times.Once);
    }
}