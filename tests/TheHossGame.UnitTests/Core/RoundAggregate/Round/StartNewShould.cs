// 🃏 The HossGame 🃏
// <copyright file="StartNewShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.RoundAggregate.Round;

using FluentAssertions;
using Moq;
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
   private Round sut = default!;

   [Theory]
   [AutoReadyGameData]
   public void HaveValidState(
      AGameId gameId,
      PlayerId firstBidder,
      Mock<IShufflingService> shuffleService)
   {
      this.Initialize(gameId, firstBidder, shuffleService);

      this.sut.Should().NotBeNull();
      this.sut.Id.Should().NotBeNull();
      this.sut.GameId.Should().Be(gameId);
      this.sut.FirstBidder.Should().Be(firstBidder);
      this.sut.State.Should().Be(Round.RoundState.Started);
      this.sut.Deck.Should().NotBeNull();
   }

   [Theory]
   [AutoReadyGameData]
   public void RaiseStartEvent(
   AGameId gameId,
   PlayerId firstBidder,
   Mock<IShufflingService> shuffleService)
   {
      this.Initialize(gameId, firstBidder, shuffleService);

      var startedEvent = this.sut.Events.ShouldContain()
      .SingleEventOfType<RoundStartedEvent>();

      startedEvent.GameId.Should().Be(gameId);
      startedEvent.RoundId.Should().NotBeNull();
      startedEvent.Deck.Should().NotBeNull();
      startedEvent.Deck.Cards.Should().HaveCount(24);
      var suitGroups = startedEvent.Deck.Cards.GroupBy(c => c.Suit)
         .Should()
         .HaveCount(4).And.Subject;
      suitGroups.Should().AllSatisfy(s => s.Should().HaveCount(6));
      startedEvent.Deck.Cards.GroupBy(c => c.Rank)
         .Should()
         .HaveCount(6);
   }

   [Theory]
   [AutoReadyGameData]
   public void CallShuffleService(
   AGameId gameId,
   PlayerId firstBidder,
   Mock<IShufflingService> shuffleService)
   {
      this.Initialize(gameId, firstBidder, shuffleService);

      shuffleService.Verify(
      s => s.Shuffle(It.IsAny<IList<Card>>()),
      Times.Once);
   }

   internal void Initialize(
      AGameId gameId,
      PlayerId firstBidder,
      Mock<IShufflingService> shuffleService)
   {
      this.sut = Round.StartNew(gameId, firstBidder, shuffleService.Object);
   }
}
