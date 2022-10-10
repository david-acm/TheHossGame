﻿// 🃏 The HossGame 🃏
// <copyright file="NoGamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using System;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public class NoGamePlayer
   : GamePlayer
{
   public NoGamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
      : base(gameId, playerId, applier)
   {
   }

   public override bool IsNull => true;

   internal override void Join(TeamId teamId)
   {
      var player = new AGamePlayer(this.GameId, this.Id, this.Applier);
      player.Join(teamId);
   }

   internal override NoGamePlayer Ready()
   {
      return this;
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}