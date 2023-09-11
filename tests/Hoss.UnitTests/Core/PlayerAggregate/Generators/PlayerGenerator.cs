// 🃏 The HossGame 🃏
// <copyright file="PlayerGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using System.Net.Mail;
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

    private Profile RandomPlayerEmail()
    {
        var playerName = this.specimenContext.Create<PlayerName>();
        var playerEmail = this.specimenContext.Create<MailAddress>().Address;
        return Profile.FromNewRegister(ProfileEmail.FromString(playerEmail), playerName);
    }
}