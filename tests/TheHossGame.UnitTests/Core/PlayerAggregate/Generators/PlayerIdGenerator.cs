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
   public object Create(object request, ISpecimenContext context)
      => request is PlayerId ? RandomPlayerName() : new NoSpecimen();

   private static object RandomPlayerName()
   {
      return new APlayerId();
   }
}