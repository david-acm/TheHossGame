// 🃏 The HossGame 🃏
// <copyright file="PlayerNameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

using AutoFixture.Kernel;
using System;
using TheHossGame.Core.PlayerAggregate;

public class PlayerNameGenerator : ISpecimenBuilder
{
   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(PlayerName).Equals(request))
      {
         return new NoSpecimen();
      }

      return RandomPlayerName();
   }

   private static object RandomPlayerName()
   {
      return new PlayerName($"Player_{DateTime.Now.Ticks}");
   }
}