// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using System;
using System.Runtime.Serialization;
using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate.Events;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class Player : EntityBase<PlayerId>, IAggregateRoot
{
   private Player(PlayerId id, PlayerName name)
      : base(id)
   {
      this.Name = name;
      this.JoiningGameId = new NoGameId();
   }

   public enum PlayerState
   {
      Playing,
      NotPlaying,
   }

   public PlayerState State { get; private set; } = PlayerState.NotPlaying;

   public PlayerName Name { get; }

   public AGameId JoiningGameId { get; private set; }

   public bool IsJoiningGame => this.JoiningGameId is GameId;

   public static Player FromRegister(PlayerId playerId, PlayerName playerName)
   {
      var player = new Player(playerId, playerName);
      var @event = new PlayerRegisteredEvent(playerId, playerName);
      player.RaiseDomainEvent(@event);

      return player;
   }

   public void RequestJoinGame(AGameId gameId)
   {
      /*This should be checked both by the client before sending the command and by a domain service after hydrating the player aggregate and before calling this method. There is a race condition where a player can join a game and tries to join another game after the condition is checked. Allowing a player to briefly join two games. This edge and rare case could be solved by:
      - Defining an SLA (?)
      - Having a separate process that checks whether an user has two games and doing some compensation
      - Decoupling the request event from the actual joining to a game. Allowing some time to catch on. Or by sending the id of the last event used to perform the check? */

      // Preconditions
      if (this.IsJoiningGame)
      {
         this.RaiseDomainEvent(new CannotJoinGameEvent(
            this.Id,
            $"Player already in a game"));
      }

      // Post conditions
      this.RaiseDomainEvent(new RequestedJoinGameEvent(gameId));
      this.State = PlayerState.Playing;
      this.JoiningGameId = gameId;

      this.EnsureValidState(); // Invariants
   }

   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         PlayerState.NotPlaying => this.JoiningGameId is NoGameId,
         PlayerState.Playing => this.JoiningGameId is not NoGameId,
         _ => true,
      };

      if (!valid)
      {
         throw new InvalidEntityStateException(this, $"Failed to validate entity {nameof(Player)}");
      }
   }

   protected override void When(DomainEventBase @event)
   {
      throw new NotImplementedException();
   }
}

[Serializable]
public class InvalidEntityStateException : Exception
{
   private readonly Player? player;
   private readonly string? message;

   public InvalidEntityStateException()
   {
   }

   public InvalidEntityStateException(string? message)
      : base(message)
   {
   }

   public InvalidEntityStateException(Player player, string message)
      : this(message)
   {
      this.player = player;
      this.message = message;
   }

   public InvalidEntityStateException(string? message, Exception? innerException)
      : base(message, innerException)
   {
   }

   protected InvalidEntityStateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
   {
   }
}