// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.Result;
using TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class Player : EntityBase<PlayerId>, IAggregateRoot
{
   public Player(PlayerId id, PlayerName name, PlayerEmail email)
      : base(id)
   {
      this.Email = email;
      this.Name = name;
   }

   public Player(PlayerName name, PlayerEmail email)
      : this(new PlayerId(), name, email)
   {
   }

   public enum PlayerState
   {
      Registered,
   }

   public PlayerState State { get; private set; }

   public PlayerName Name { get; }

   public PlayerEmail Email { get; }

   public Result<Player> Register()
   {
      var @event = new PlayerRegisteredEvent(this.Id, this.Email, this.Name);
      this.RegisterDomainEvent(@event);
      return Result.Success(this);
   }
}
