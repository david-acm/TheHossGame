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

public abstract class Player : EntityBase<PlayerId>, IAggregateRoot
{
   protected Player(PlayerId id)
      : base(id)
   {
      this.JoiningGameId = new NoGameId();
   }

   protected bool IsJoiningGame => this.JoiningGameId is AGameId;

   protected GameId JoiningGameId { get; set; }
}

public sealed class APlayer : Player
{
   private APlayer(PlayerId id)
      : base(id)
   {
   }

   private enum PlayerState
   {
      Playing,
      NotPlaying,
   }

   protected override bool IsNull => false;

   private PlayerState State { get; set; } = PlayerState.NotPlaying;

   public static APlayer FromRegister(PlayerId playerId, PlayerName playerName)
   {
      var player = new APlayer(playerId);
      var @event = new PlayerRegisteredEvent(playerId, playerName);
      player.RaiseDomainEvent(@event);

      return player;
   }

   public void RequestJoinGame(GameId gameId)
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
            "APlayer already in a game"));
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