// 🃏 The HossGame 🃏
// <copyright file="SelectTrumpShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using TheHossGame.UnitTests.Extensions;

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.SharedKernel;
using PlayerAggregate.Generators;
using Xunit;
using static Hoss.Core.GameAggregate.RoundEntity.RoundEvents;

#endregion

public class SelectTrumpShould
{
    [Theory]
    [BidFinishedGameData]
    public void RaiseTrumpSelectedEvent(AGame game)
    {
        var cardSuit = Suit.Diamonds;
        game.SelectTrump(game.CurrentPlayerId, cardSuit);

        game.Events.ShouldContain().SingleEventOfType<TrumpSelectedEvent>().Should().BeAssignableTo<RoundEventBase>();

        game.CurrentRoundView.TrumpSelected.Should().Be(cardSuit);
    }

    [Theory]
    [PlayerData]
    public void NotRaiseTrumpSelectedEventWhenGameNotReady(AGame game)
    {
        game.SelectTrump(game.CurrentPlayerId, Suit.Hearts);

        game.Events.ShouldContain().NoEventsOfType<TrumpSelectedEvent>();

        game.CurrentRoundView.TrumpSelected.Should().Be(Suit.None);
    }

    [Theory]
    [BidFinishedGameData]
    public void ThrowInvalidOperationExceptionWhenPlayerIsNotBidWinner(AGame game)
    {
        var outOfTurnPlayer = game.FindGamePlayers(TeamId.EastWest).First().PlayerId;
        var selectTrumpAction = () => game.SelectTrump(outOfTurnPlayer, Suit.Hearts);

        selectTrumpAction.Should().Throw<InvalidEntityStateException>();

        game.Events.ShouldContain().NoEventsOfType<TrumpSelectedEvent>();

        game.CurrentRoundView.TrumpSelected.Should().Be(Suit.None);
    }
}