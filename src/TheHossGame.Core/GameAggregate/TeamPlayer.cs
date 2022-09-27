// 🃏 The HossGame 🃏
// <copyright file="TeamPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using System;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public abstract class TeamPlayer : EntityBase<PlayerId>
{
   protected TeamPlayer(PlayerId playerId, TeamId teamId, Action<DomainEventBase> applier)
      : base(playerId, applier)
   {
      this.PlayerId = playerId;
      this.TeamId = teamId;
   }

   public bool IsReady { get; protected set; }

   public PlayerId PlayerId { get; protected set; }

   public TeamId TeamId { get; protected set; }
}

public class ATeamPlayer
   : TeamPlayer
{
   public ATeamPlayer(PlayerId PlayerId, TeamId TeamId, Action<DomainEventBase> applier)
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

public class NoTeamPlayer
   : TeamPlayer
{
   public NoTeamPlayer()
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
