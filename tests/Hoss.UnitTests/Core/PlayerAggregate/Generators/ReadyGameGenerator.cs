// 🃏 The HossGame 🃏
// <copyright file="ReadyGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.Kernel;
using Hoss.Core.GameAggregate;
using Hoss.Core.Interfaces;
using Moq;

#endregion

public class ReadyGameGenerator : ISpecimenBuilder
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

        return GenerateReadyGame();
    }

    #endregion

    private AGame GenerateReadyGame()
    {
        var shufflingService = specimenContext!.Create<Mock<IShufflingService>>();
        var playerIds = specimenContext.Create<IEnumerable<APlayerId>>().ToList();
        
        var readyGame = AGame.CreateForPlayer(new AGameId(Guid.NewGuid()), playerIds.First(), shufflingService.Object);

        readyGame.JoinPlayerToTeam(playerIds[1], TeamId.NorthSouth);
        readyGame.JoinPlayerToTeam(playerIds[2], TeamId.EastWest);
        readyGame.JoinPlayerToTeam(playerIds[3], TeamId.EastWest);

        playerIds.ForEach(id => readyGame.TeamPlayerReady(id));

        return readyGame;
    }
}