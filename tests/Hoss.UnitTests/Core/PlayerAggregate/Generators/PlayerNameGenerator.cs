// 🃏 The HossGame 🃏
// <copyright file="PlayerNameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

#region

using AutoFixture.Kernel;
using Hoss.Core.PlayerAggregate;

#endregion

internal class PlayerNameGenerator : ISpecimenBuilder
{
   #region ISpecimenBuilder Members

   public object Create(object request, ISpecimenContext context)
   {
      if (!typeof(PlayerName).Equals(request))
      {
         return new NoSpecimen();
      }

      return RandomPlayerName();
   }

   #endregion

   private static object RandomPlayerName()
   {
      return new PlayerName($"Player_{DateTime.Now.Ticks}");
   }
}
