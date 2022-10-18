// 🃏 The HossGame 🃏
// <copyright file="PlayerEmailGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using System.Net.Mail;
using AutoFixture;
using AutoFixture.Kernel;
using TheHossGame.Core.PlayerAggregate;

internal class PlayerEmailGenerator : ISpecimenBuilder
{
   private ISpecimenContext? specimenContext;

   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      this.specimenContext = context;
      if (!typeof(PlayerEmail).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.RandomPlayerEmail();
   }

   #endregion

   private object RandomPlayerEmail()
   {
      var address = this.specimenContext.Create<MailAddress>().Address;
      return PlayerEmail.FromString(address);
   }
}
