// 🃏 The HossGame 🃏
// <copyright file="PlayerEmailGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture;
using AutoFixture.Kernel;
using System.Net.Mail;
using TheHossGame.Core.PlayerAggregate;

internal class PlayerEmailGenerator : ISpecimenBuilder
{
   private ISpecimenContext? specimenContext;

   public object Create(object request, ISpecimenContext context)
   {
      this.specimenContext = context;
      if (!typeof(PlayerEmail).Equals(request))
      {
         return new NoSpecimen();
      }

      return this.RandomPlayerEmail();
   }

   private object RandomPlayerEmail()
   {
      string address = this.specimenContext.Create<MailAddress>().Address;
      return PlayerEmail.FromString(address);
   }
}
