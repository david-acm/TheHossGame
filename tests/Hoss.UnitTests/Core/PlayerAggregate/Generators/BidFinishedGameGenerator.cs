// 🃏 The HossGame 🃏
// <copyright file="BidFinishedGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.Kernel;
using Hoss.Core.GameAggregate;
using Hoss.Core.Interfaces;
using Moq;
using GameAggregate.Round;

#endregion

public class BidFinishedGameGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        specimenContext = context;
        if (!typeof(AGame).Equals(request))
        {
            return new NoSpecimen();
        }

        return GenerateBidFinishedGame();
    }

    #endregion

    private AGame GenerateBidFinishedGame()
    {
        var shufflingService = specimenContext!.Create<Mock<IShufflingService>>();
        var playerIds = specimenContext.Create<IEnumerable<APlayerId>>().ToList();

        var game = AGame.CreateForPlayer(new AGameId(Guid.NewGuid()), playerIds.First(), shufflingService.Object);

        game.JoinPlayerToTeam(playerIds[1], TeamId.NorthSouth);
        game.JoinPlayerToTeam(playerIds[2], TeamId.EastWest);
        game.JoinPlayerToTeam(playerIds[3], TeamId.EastWest);

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
        specimenContext = context;
        if (!typeof(AGame).Equals(request)) return new NoSpecimen();

        return GenerateBidWithHossGame();
    }

    #endregion

    private AGame GenerateBidWithHossGame()
    {
        var shufflingService = specimenContext!.Create<Mock<IShufflingService>>();
        var playerIds = specimenContext.Create<IEnumerable<APlayerId>>().ToList();

        var game = AGame.CreateForPlayer(new AGameId(Guid.NewGuid()), playerIds.First(), shufflingService.Object);
        game.JoinPlayerToTeam(playerIds[1], TeamId.NorthSouth);
        
        game.JoinPlayerToTeam(playerIds[2], TeamId.EastWest);
        game.JoinPlayerToTeam(playerIds[3], TeamId.EastWest);

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