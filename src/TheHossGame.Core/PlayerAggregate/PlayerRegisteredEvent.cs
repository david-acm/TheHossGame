// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisteredEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.SharedKernel;

public record PlayerRegisteredEvent : DomainEventBase
{
   public PlayerRegisteredEvent(
      PlayerId playerId,
      PlayerEmail playerEmail,
      PlayerName playerName)
   {
      this.PlayerId = playerId;
      this.PlayerEmail = playerEmail;
      this.PlayerName = playerName;
   }

   public delegate PlayerRegisteredEvent Factory(
      PlayerId playerId,
      PlayerEmail playerEmail,
      PlayerName playerName);

   public PlayerId PlayerId { get; }

   public PlayerEmail PlayerEmail { get; }

   public PlayerName PlayerName { get; }
}
