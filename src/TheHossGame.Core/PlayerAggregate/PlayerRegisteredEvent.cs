// 🃏 The HossGame 🃏
// <copyright file="PlayerRegisteredEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.SharedKernel;

public class PlayerRegisteredEvent : DomainEventBase
{
   public PlayerRegisteredEvent(PlayerId playerId, PlayerEmail playerEmail, PlayerName playerName)
   {
      this.PlayerId = playerId;
      this.PlayerEmail = playerEmail;
      this.PlayerName = playerName;
   }

   public PlayerId PlayerId { get; private set; }

   public PlayerEmail PlayerEmail { get; private set; }

   public PlayerName PlayerName { get; private set; }
}
