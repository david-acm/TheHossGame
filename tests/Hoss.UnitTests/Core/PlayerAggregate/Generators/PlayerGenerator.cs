// 🃏 The HossGame 🃏
// <copyright file="PlayerGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture;
using AutoFixture.Kernel;
using Hoss.Core.PlayerAggregate;

#endregion

public class PlayerGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        this.specimenContext = context;
        return typeof(Base).Equals(request) ? this.RandomPlayerEmail() : new NoSpecimen();
    }

    #endregion

    private Player RandomPlayerEmail()
    {
        var playerId = this.specimenContext.Create<APlayerId>();
        var playerName = this.specimenContext.Create<PlayerName>();
        return Player.FromRegister(playerId, playerName);
    }
}