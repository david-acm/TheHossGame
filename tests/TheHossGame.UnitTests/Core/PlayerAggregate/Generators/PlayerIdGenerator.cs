// 🃏 The HossGame 🃏
// <copyright file="PlayerIdGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture.Kernel;
using TheHossGame.Core.PlayerAggregate;

internal class PlayerIdGenerator : ISpecimenBuilder
{
   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      return request is PlayerId ? RandomPlayerName() : new NoSpecimen();
   }

   #endregion

   private static object RandomPlayerName()
   {
      return new APlayerId();
   }
}
