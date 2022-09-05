// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationServiceConstructorShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.Services;
using Xunit;

public class PlayerRegistrationServiceConstructorShould
{
   [Theory]
   [AutoMoqData]
   public void CreateNewService(PlayerRegistrationService service) =>
      service.Should().NotBeNull();
}
