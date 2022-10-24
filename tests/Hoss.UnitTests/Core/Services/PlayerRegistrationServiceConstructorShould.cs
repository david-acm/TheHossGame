// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationServiceConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

#region

using FluentAssertions;
using Hoss.Core.Services;
using Xunit;

#endregion

public class PlayerRegistrationServiceConstructorShould
{
   [Theory]
   [AutoMoqData]
   public void CreateNewService(PlayerRegistrationService service)
   {
      service.Should().NotBeNull();
   }
}
