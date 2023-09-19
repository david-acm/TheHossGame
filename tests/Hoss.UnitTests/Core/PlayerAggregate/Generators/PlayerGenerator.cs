// 🃏 The HossGame 🃏
// <copyright file="PlayerGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.ProfileAggregate;

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using System.Net.Mail;
using AutoFixture;
using AutoFixture.Kernel;

#endregion

public class PlayerGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        specimenContext = context;
        return typeof(Base).Equals(request) ? RandomPlayerEmail() : new NoSpecimen();
    }

    #endregion

    private Profile RandomPlayerEmail()
    {
        var playerName = specimenContext.Create<PlayerName>();
        var playerEmail = specimenContext.Create<MailAddress>().Address;
        return Profile.FromNewRegister(ProfileEmail.FromString(playerEmail), playerName, new AProfileId());
    }
}