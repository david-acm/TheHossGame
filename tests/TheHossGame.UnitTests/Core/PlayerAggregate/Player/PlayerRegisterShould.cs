﻿// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Player;

using FluentAssertions;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

public class RegisterShould
{
   [Theory]
   [AutoPlayerData]
   public void RaisePlayerRegisteredEvent(
      APlayerId playerId,
      PlayerName playerName)
   {
      var player = APlayer.FromRegister(playerId, playerName);

      var @event = player.Events.Should().ContainSingle()
                         .Subject.As<PlayerRegisteredEvent>();
      @event = @event.Should().BeOfType<PlayerRegisteredEvent>().Subject;
      @event.PlayerId.Should().NotBeNull();
      @event.PlayerName.Should().NotBeNull();
   }
}
