// 🃏 The HossGame 🃏
// <copyright file="GamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.PlayerEntity;

using System;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public abstract class GamePlayer : EntityBase<PlayerId>
{
   protected GamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
      : base(playerId, applier)
   {
      this.GameId = gameId;
      this.PlayerId = playerId;
   }

   public bool IsReady { get; protected set; }

   public GameId GameId { get; }

   public PlayerId PlayerId { get; protected set; }

   public TeamId TeamId { get; protected set; }

   internal abstract void Join(TeamId teamId);

   internal abstract GamePlayer Ready();
}
