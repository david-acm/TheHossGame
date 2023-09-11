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
        this.specimenContext = context;
        if (!typeof(AGame).Equals(request))
        {
            return new NoSpecimen();
        }

        return this.GenerateReadyGame();
    }

    #endregion

    private AGame GenerateReadyGame()
    {
        var shufflingService = this.specimenContext!.Create<Mock<IShufflingService>>();
        var playerIds = this.specimenContext.Create<IEnumerable<APlayerId>>().ToList();

        var readyGame = AGame.CreateForPlayer(playerIds.First(), shufflingService.Object);

        readyGame.JoinPlayerToTeam(playerIds[1], Game.TeamId.Team1);
        readyGame.JoinPlayerToTeam(playerIds[2], Game.TeamId.Team2);
        readyGame.JoinPlayerToTeam(playerIds[3], Game.TeamId.Team2);

        playerIds.ForEach(id => readyGame.TeamPlayerReady(id));

        return readyGame;
    }
}