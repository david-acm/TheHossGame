// 🃏 The HossGame 🃏
// <copyright file="PlayerEnumerableGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture.Kernel;
using Hoss.Core.GameAggregate;
using Hoss.Core.PlayerAggregate;

#endregion

public class PlayerEnumerableGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        this.specimenContext = context;
        return typeof(IEnumerable<Base>).Equals(request) ? this.GeneratePlayerList() : new NoSpecimen();
    }

    #endregion

    private List<Base> GeneratePlayerList()
    {
        var playerList = new List<Base>
        {
            this.GeneratePLayer(),
            this.GeneratePLayer(),
            this.GeneratePLayer(),
            this.GeneratePLayer(),
        };

        return playerList;
    }

    private Base GeneratePLayer()
    {
        return (Base) new PlayerGenerator().Create(typeof(Base), this.specimenContext!);
    }
}

public class PlayerIdEnumerableGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        this.specimenContext = context;
        if (typeof(IEnumerable<APlayerId>).Equals(request))
            return this.GeneratePlayerList();
        return new NoSpecimen();
    }

    #endregion

    private List<APlayerId> GeneratePlayerList()
    {
        var playerList = new List<APlayerId>
        {
            this.GeneratePlayerId(),
            this.GeneratePlayerId(),
            this.GeneratePlayerId(),
            this.GeneratePlayerId(),
        };

        return playerList;
    }

    private APlayerId GeneratePlayerId()
    {
        return new APlayerId();
    }
}