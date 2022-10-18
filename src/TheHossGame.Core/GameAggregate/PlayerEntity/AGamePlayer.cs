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
   internal AGamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
      : base(gameId, playerId, applier)
   {
   }

   protected override bool IsNull => false;

   private bool HasJoinedGame => this.TeamId != TeamId.NoTeamId;

   internal static AGamePlayer FromStream(PlayerJoinedEvent @event, Action<DomainEventBase> applier)
   {
      (GameId gameId, PlayerId playerId, TeamId teamId) = @event;
      return new AGamePlayer(gameId, playerId, applier)
      {
         TeamId = teamId,
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

   internal override void Ready()
   {
      var @event = new PlayerReadyEvent(this.GameId, this.Id);
      this.Apply(@event);
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
      switch (@event)
      {
         case PlayerJoinedEvent(_, var playerId, var teamId):
            this.PlayerId = playerId;
            this.TeamId = teamId;
            break;
         case PlayerReadyEvent:
            this.IsReady = true;
            break;
      }
   }
}
