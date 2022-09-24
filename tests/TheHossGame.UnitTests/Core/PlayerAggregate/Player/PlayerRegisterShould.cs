﻿// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.UnitTests.Core.Services;
using Xunit;
using Events = TheHossGame.Core.PlayerAggregate.Events;

public class RegisterShould
{
   [Theory]
   [AutoPlayerData]
   [AutoMoqData]
   public void RaisePlayerRegisteredEvent(
       PlayerId playerId,
       PlayerName playerName)
   {
      var player = Player.FromRegister(playerId, playerName);

      var @event = player.DomainEvents.Should().ContainSingle()
         .Subject.As<PlayerRegisteredEvent>();
      @event = @event.Should().BeOfType<Events.PlayerRegisteredEvent>().Subject;
      @event.PlayerId.Should().NotBeNull();
      @event.PlayerName.Should().NotBeNull();
   }
}
