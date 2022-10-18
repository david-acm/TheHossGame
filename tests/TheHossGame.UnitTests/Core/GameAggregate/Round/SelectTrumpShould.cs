// 🃏 The HossGame 🃏
// <copyright file="SelectTrumpShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

using FluentAssertions;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using TheHossGame.UnitTests.Extensions;
using Xunit;

public class SelectTrumpShould
{
   [Theory]
   [AutoBidFinishedGameData]
   public void RaiseTrumpSelectedEvent(AGame game)
   {
      game.SelectTrump(game.CurrentPlayerId, CardSuit.Hearts);

      game.Events.ShouldContain().SingleEventOfType<TrumpSelectedEvent>();

      game.CurrentRound.TrumpSelected.Should().Be(CardSuit.Hearts);
   }

   [Theory]
   [AutoBidFinishedGameData]
   public void ThrowInvalidOperationExceptionWhenPlayerIsNotBidWinner(AGame game)
   {
      var outOfTurnPlayer = game.FindGamePlayers(Game.TeamId.Team2).First().PlayerId;
      var selectTrumpAction = () => game.SelectTrump(outOfTurnPlayer, CardSuit.Hearts);

      selectTrumpAction.Should().Throw<InvalidEntityStateException>();

      game.Events.ShouldContain().NoEventsOfType<TrumpSelectedEvent>();

      game.CurrentRound.TrumpSelected.Should().Be(CardSuit.Hearts);
   }
}