// 🃏 The HossGame 🃏
// <copyright file="PlayerEnumerableGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.ProfileAggregate;

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture.Kernel;
using Hoss.Core.GameAggregate;

#endregion

public class PlayerEnumerableGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        specimenContext = context;
        return typeof(IEnumerable<Base>).Equals(request) ? GeneratePlayerList() : new NoSpecimen();
    }

    #endregion

    private List<Base> GeneratePlayerList()
    {
        var playerList = new List<Base>
        {
            GeneratePlayer(),
            GeneratePlayer(),
            GeneratePlayer(),
            GeneratePlayer(),
        };

        return playerList;
    }

    private Base GeneratePlayer()
    {
        return (Base) new PlayerGenerator().Create(typeof(Base), specimenContext!);
    }
}

public class PlayerIdEnumerableGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        specimenContext = context;
        if (typeof(IEnumerable<APlayerId>).Equals(request))
            return GeneratePlayerList();
        return new NoSpecimen();
    }

    #endregion

    private List<APlayerId> GeneratePlayerList()
    {
        var playerList = new List<APlayerId>
        {
            GeneratePlayerId(),
            GeneratePlayerId(),
            GeneratePlayerId(),
            GeneratePlayerId(),
        };

        return playerList;
    }

    private APlayerId GeneratePlayerId()
    {
        return new APlayerId(Guid.NewGuid());
    }
}