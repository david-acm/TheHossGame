// 🃏 The HossGame 🃏
// <copyright file="BidShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
namespace TheHossGame.UnitTests.Core.RoundAggregate.Round;

using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;
using static TheHossGame.Core.GameAggregate.Game.TeamId;
using Round = TheHossGame.Core.RoundAggregate.Round;

public class BidShould
{
   [Theory]
   [AutoReadyGameData]
   public void RaiseBidEvent(Round round)
   {
      var bidCommand = new BidCommand(round.TeamPlayers[0].PlayerId, BidValue.One);
      round.Bid(bidCommand);
      var @event = round.Events.ShouldContain()
         .SingleEventOfType<BidEvent>();

      @event.GameId.Should().Be(round.GameId);
      @event.RoundId.Should().Be(round.Id);
      @event.Bid.PlayerId.Should().Be(bidCommand.PlayerId);
      @event.Bid.Value.Should().Be(bidCommand.Value);
   }

   [Theory]
   [AutoReadyGameData]
   public void AddBidToRound(Round round)
   {
      var bidCommand = new BidCommand(round.TeamPlayers[0].PlayerId, BidValue.One);
      round.Bid(bidCommand);
      round.State.Should().Be(Round.RoundState.CardsDealt);
      var bid = round.Bids.Should().ContainSingle().Subject;
      bid.PlayerId.Should().Be(bidCommand.PlayerId);
      bid.Value.Should().Be(bidCommand.Value);
   }

   [Theory]
   [AutoReadyGameData]
   public void AllowPlayersToPass(
      Round round)
   {
      round.Bid(new BidCommand(round.CurrentPlayerId, BidValue.One));
      round.Bid(new BidCommand(round.CurrentPlayerId, BidValue.Two));
      round.Bid(new BidCommand(round.CurrentPlayerId, BidValue.Pass));

      round.Bids.Should().HaveCount(3);
   }

   [Theory]
   [AutoReadyGameData]
   public void ThrowInvalidEntityExceptionWhenBidLowerThanOthers(
      Round round)
   {
      round.Bid(new BidCommand(round.CurrentPlayerId, BidValue.One));
      round.Bid(new BidCommand(round.CurrentPlayerId, BidValue.Two));

      var bidAction = () => round.Bid(new BidCommand(round.CurrentPlayerId, BidValue.One));

      bidAction.Should().Throw<InvalidEntityStateException>();
   }

   [Theory]
   [AutoReadyGameData]
   public void PlayerOrderShouldBeCorrect(Round round)
   {
      round.TeamPlayers.Should().ContainInOrder(
         round.TeamPlayers.First(p => p.TeamId == Team1),
         round.TeamPlayers.First(p => p.TeamId == Team2),
         round.TeamPlayers.Last(p => p.TeamId == Team1),
         round.TeamPlayers.Last(p => p.TeamId == Team2));
   }

   [Theory]
   [AutoReadyGameData]
   public void ThrowInvalidEntityExceptionWhenOutOfTurn(Round round)
   {
      round.Bid(new BidCommand(round.TeamPlayers[0].PlayerId, BidValue.One));

      var bidAction = () => round.Bid(new BidCommand(round.TeamPlayers[2].PlayerId, BidValue.Two));

      bidAction.Should().Throw<InvalidEntityStateException>();
   }

   [Theory]
   [AutoReadyGameData]
   public void ThrowInvalidEntityExceptionWhenPLayerNotInGame(Round round, APlayer player)
   {
      var bidAction = () => round.Bid(new BidCommand(player.Id, BidValue.One));

      bidAction.Should().Throw<InvalidEntityStateException>();
   }
}
