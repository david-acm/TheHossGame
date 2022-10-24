// 🃏 The HossGame 🃏
// <copyright file="NoGamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.PlayerEntity;

   #region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;
using static Game;

#endregion

public sealed class NoGamePlayer : GamePlayer
{
   public NoGamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
      : base(gameId, playerId, applier)
   {
   }

   internal override void Join(TeamId teamId)
   {
      var player = new AGamePlayer(this.GameId, this.Id, this.Applier);
      player.Join(teamId);
      this.When(new PlayerJoinedEvent(this.GameId, this.Id, teamId));
      this.EnsureValidState();
   }

   internal override void Ready()
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}
