// 🃏 The HossGame 🃏
// <copyright file="RoundPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public record RoundPlayer
   : ValueObject
{
   public RoundPlayer(PlayerId PlayerId, TeamId TeamId)
   {
      this.PlayerId = PlayerId;
      this.TeamId = TeamId;
   }

   public PlayerId PlayerId { get; }
   public TeamId TeamId { get; }
}
