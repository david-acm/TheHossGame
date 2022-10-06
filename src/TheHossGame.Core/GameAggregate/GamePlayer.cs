// 🃏 The HossGame 🃏
// <copyright file="GamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using System;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public abstract class GamePlayer : EntityBase<PlayerId>
{
   protected GamePlayer(PlayerId playerId, TeamId teamId, Action<DomainEventBase> applier)
      : base(playerId, applier)
   {
      this.PlayerId = playerId;
      this.TeamId = teamId;
   }

   public bool IsReady { get; protected set; }

   public PlayerId PlayerId { get; protected set; }

   public TeamId TeamId { get; protected set; }
}

public class AGamePlayer
   : GamePlayer
{
   public AGamePlayer(PlayerId PlayerId, TeamId TeamId, Action<DomainEventBase> applier)
      : base(PlayerId, TeamId, applier)
   {
   }

   protected override void EnsureValidState()
   {
      throw new NotImplementedException();
   }

   protected override void When(DomainEventBase @event)
   {
      switch (@event)
      {
         case PlayerJoinedEvent e:
            this.PlayerId = e.PlayerId;
            this.TeamId = e.TeamId;
            break;
         case PlayerReadyEvent e:
            this.IsReady = true;
            break;
         default:
            break;
      }
   }
}

public class NoGamePlayer
   : GamePlayer
{
   public NoGamePlayer()
      : base(new NoPlayerId(), TeamId.NoTeamId, (o) => { })
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}
