// 🃏 The HossGame 🃏
// <copyright file="AGamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.PlayerEntity;

using System;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public class AGamePlayer
   : GamePlayer
{
   internal AGamePlayer(GameId GameId, PlayerId PlayerId, Action<DomainEventBase> applier)
      : base(GameId, PlayerId, applier)
   {
   }

   public override bool IsNull => false;

   public bool HasJoinedGame => this.TeamId != TeamId.NoTeamId;

   internal static AGamePlayer FromStream(PlayerJoinedEvent @event, Action<DomainEventBase> applier)
   {
      return new AGamePlayer(@event.gameId, @event.PlayerId, applier)
      {
         TeamId = @event.TeamId,
      };
   }

   internal override void Join(TeamId teamId)
   {
      if (this.HasJoinedGame)
      {
         this.Apply(new PlayerAlreadyInGame(this.Id));
         return;
      }

      var @event = new PlayerJoinedEvent(this.GameId, this.Id, teamId);
      this.Apply(@event);
   }

   internal override AGamePlayer Ready()
   {
      var @event = new PlayerReadyEvent(this.GameId, this.Id);
      this.Apply(@event);

      return this;
   }

   protected override void EnsureValidState()
   {
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
