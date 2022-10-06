// 🃏 The HossGame 🃏
// <copyright file="TeamPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public record TeamPlayer
   : ValueObject
{
   public TeamPlayer(PlayerId PlayerId, TeamId TeamId)
   {
      this.PlayerId = PlayerId;
      this.TeamId = TeamId;
   }

   public PlayerId PlayerId { get; }
   public TeamId TeamId { get; }
}
