// 🃏 The HossGame 🃏
// <copyright file="PlayerEmailGenerator.cs" company="Reactive">
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

internal class PlayerEmailGenerator : ISpecimenBuilder
{
    private ISpecimenContext? specimenContext;

    #region ISpecimenBuilder Members

    public object Create(object request, ISpecimenContext context)
    {
        specimenContext = context;
        if (!typeof(ProfileEmail).Equals(request))
        {
            return new NoSpecimen();
        }

        return RandomPlayerEmail();
    }

    #endregion

    private object RandomPlayerEmail()
    {
        var address = specimenContext.Create<MailAddress>().Address;
        return ProfileEmail.FromString(address);
    }
}