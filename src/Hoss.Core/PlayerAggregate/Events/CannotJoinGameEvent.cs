// 🃏 The HossGame 🃏
// <copyright file="CannotJoinGameEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate.Events;

#region

using Hoss.SharedKernel;

#endregion

public record CannotJoinGameEvent(PlayerId PlayerId, string Reason) : PlayerEventBase(PlayerId);

public abstract record PlayerEventBase : DomainEventBase
{
   protected PlayerEventBase(PlayerId playerId)
      : base(playerId)
   {
      this.PlayerId = playerId;
   }

   public PlayerId PlayerId { get; }
}
