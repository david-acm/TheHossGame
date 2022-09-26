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

public abstract class Player : EntityBase<APlayerId>, IAggregateRoot
{
   protected Player(APlayerId id)
      : base(id)
   {
      this.JoiningGameId = new NoGameId();
   }

   public bool IsJoiningGame => this.JoiningGameId is GameId;

   public AGameId JoiningGameId { get; protected set; }

   public abstract void RequestJoinGame(AGameId gameId);
}

public class NoPlayer : Player
{
   public NoPlayer()
      : base(new APlayerId())
   {
   }

   public override void RequestJoinGame(AGameId gameId)
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}

public class APlayer : Player
{
   private APlayer(APlayerId id, PlayerName name)
      : base(id)
   {
      this.Name = name;
   }

   public enum PlayerState
   {
      Playing,
      NotPlaying,
   }

   public PlayerState State { get; private set; } = PlayerState.NotPlaying;

   public PlayerName Name { get; }

   public static APlayer FromRegister(APlayerId playerId, PlayerName playerName)
   {
      var player = new APlayer(playerId, playerName);
      var @event = new PlayerRegisteredEvent(playerId, playerName);
      player.RaiseDomainEvent(@event);

      return player;
   }

   public override void RequestJoinGame(AGameId gameId)
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
            $"APlayer already in a game"));
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
         throw new InvalidEntityStateException(this, $"Failed to validate entity {nameof(APlayer)}");
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
   private readonly APlayer? player;
   private readonly string? message;

   public InvalidEntityStateException()
   {
   }

   public InvalidEntityStateException(string? message)
      : base(message)
   {
   }

   public InvalidEntityStateException(APlayer player, string message)
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