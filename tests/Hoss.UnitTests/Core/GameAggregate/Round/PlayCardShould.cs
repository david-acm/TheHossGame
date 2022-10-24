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

public class PlayCardShould
{
   [Theory]
   [AutoBidFinishedGameData]
   public void RaiseEventWhenNoRoundActive(AGame game)
   {
      game.SelectTrump(game.CurrentPlayerId, CardSuit.Hearts);
      game.PlayCard(game.CurrentPlayerId, new ACard(CardSuit.Clubs, CardRank.King));

      game.Events.ShouldContain().SingleEventOfType<CardPlayedEvent>();
   }

   [Theory]
   [AutoPlayerData]
   public void NotRaiseEventWhenNoRoundActive(AGame game)
   {
      game.PlayCard(game.CurrentPlayerId, new ACard(CardSuit.Clubs, CardRank.King));

      game.Events.ShouldContain().NoEventsOfType<CardPlayedEvent>();
   }

   [Theory]
   [AutoBidFinishedGameData]
   public void ThrowExceptionWhenPlayerNotInTurn(AGame game)
   {
      game.SelectTrump(game.CurrentPlayerId, CardSuit.Hearts);
      var playerNotInTurn = game.FindGamePlayers().FirstOrDefault(p => p.Id != game.CurrentPlayerId);

      var playAction = () => game.PlayCard(playerNotInTurn!.PlayerId, new ACard(CardSuit.Clubs, CardRank.King));

      playAction.Should().Throw<InvalidEntityStateException>();
   }
}
