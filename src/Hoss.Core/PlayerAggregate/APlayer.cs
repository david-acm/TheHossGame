// 🃏 The HossGame 🃏
// <copyright file="APlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate;

   #region

using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.Events;
using Hoss.Core.PlayerAggregate.Events;
using Hoss.SharedKernel;

#endregion

public sealed class APlayer : Player
{
   private APlayer(PlayerId id)
      : base(id)
   {
   }

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
         this.Apply(new CannotJoinGameEvent(this.Id, "APlayer already in a game"));
      }

      // Post conditions
      this.Apply(new RequestedJoinGameEvent(this.Id, gameId));

      this.EnsureValidState(); // Invariants
   }

   protected override void EnsureValidState()
   {
#pragma warning disable CS8509
      var valid = this.State switch
#pragma warning restore CS8509
      {
         PlayerState.Playing => this.JoiningGameId is not NoGameId,
      };

      if (!valid)
      {
         throw new InvalidEntityStateException($"Failed to validate entity {nameof(APlayer)}");
      }
   }

   protected override void When(DomainEventBase @event)
   {
#pragma warning disable CS8509
      (@event switch
#pragma warning restore CS8509
      {
         CannotJoinGameEvent => (Action)(() => { }),
         RequestedJoinGameEvent e => () =>
         {
            this.State = PlayerState.Playing;
            this.JoiningGameId = e.GameId;
         },
      }).Invoke();
   }

   #region Nested type: PlayerState

   private enum PlayerState
   {
      Playing,
      NotPlaying,
   }

   #endregion
}

public sealed class NoPlayer : Player
{
   /// <inheritdoc />
   public NoPlayer(PlayerId id)
      : base(id)
   {
      this.EnsureValidState();
      this.When(new PlayerReadyEvent(new NoGameId(), id));
   }

   /// <inheritdoc />
   protected override void When(DomainEventBase @event)
   {
   }

   /// <inheritdoc />
   protected override void EnsureValidState()
   {
   }
}
