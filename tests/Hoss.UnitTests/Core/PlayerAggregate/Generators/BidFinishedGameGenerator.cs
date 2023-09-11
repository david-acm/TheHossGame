// 🃏 The HossGame 🃏
// <copyright file="BidFinishedGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.Kernel;
using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.Interfaces;
using Moq;
using TheHossGame.UnitTests.Core.GameAggregate.Round;

#endregion

public class BidFinishedGameGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        this.specimenContext = context;
        if (!typeof(AGame).Equals(request))
        {
            return new NoSpecimen();
        }

        return this.GenerateBidFinishedGame();
    }

    #endregion

    private AGame GenerateBidFinishedGame()
    {
        var shufflingService = this.specimenContext!.Create<Mock<IShufflingService>>();
        var playerIds = this.specimenContext.Create<IEnumerable<APlayerId>>().ToList();

        var game = AGame.CreateForPlayer(playerIds.First(), shufflingService.Object);

        game.JoinPlayerToTeam(playerIds[1], Game.TeamId.Team1);
        game.JoinPlayerToTeam(playerIds[2], Game.TeamId.Team2);
        game.JoinPlayerToTeam(playerIds[3], Game.TeamId.Team2);

        playerIds.ForEach(playerId => game.TeamPlayerReady(playerId));

        game.Bid(game.CurrentPlayerId, BidValue.One);
        game.Bid(game.CurrentPlayerId, BidValue.Two);
        game.Bid(game.CurrentPlayerId, BidValue.Three);
        game.Bid(game.CurrentPlayerId, BidValue.Pass);

        return game;
    }
}

public class BidWithHossGameGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        this.specimenContext = context;
        if (!typeof(AGame).Equals(request)) return new NoSpecimen();

        return this.GenerateBidWithHossGame();
    }

    #endregion

    private AGame GenerateBidWithHossGame()
    {
        var shufflingService = this.specimenContext!.Create<Mock<IShufflingService>>();
        var playerIds = this.specimenContext.Create<IEnumerable<APlayerId>>().ToList();

        var game = AGame.CreateForPlayer(playerIds.First(), shufflingService.Object);

        game.JoinPlayerToTeam(playerIds[1], Game.TeamId.Team1);
        game.JoinPlayerToTeam(playerIds[2], Game.TeamId.Team2);
        game.JoinPlayerToTeam(playerIds[3], Game.TeamId.Team2);

        playerIds.ForEach(id => game.TeamPlayerReady(id));

        game.Bid(game.CurrentPlayerId, BidValue.One);
        game.Bid(game.CurrentPlayerId, BidValue.Two);

        var discardCard = game.GetPlayerCards(game.CurrentPlayerId).First();
        game.RequestHoss(game.CurrentPlayerId, discardCard);

        var bestCard = game.GetPlayerCards(game.CurrentPlayerId).First();
        game.GiveHossCard(game.CurrentPlayerId, bestCard);

        return game;
    }
}