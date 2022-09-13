// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using Ardalis.Result;
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

   public PlayerName Name { get; }

   public PlayerEmail Email { get; }

   public static Result<Player> Register(RegisterCommand command)
   {
      Player player = new (command.Name, command.Email);
      player.RegisterDomainEvent(
         new Events.PlayerRegisteredEvent(player.Id, command.Email, command.Name));
      return Result.Success(player);
   }
}
