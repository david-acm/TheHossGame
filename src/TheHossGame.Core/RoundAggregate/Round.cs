// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public class Round : AggregateRoot<RoundId>
{
   private Round(GameId gameId, PlayerId firstBidder)
      : base(new RoundId())
   {
      this.GameId = gameId;
      this.FirstBidder = firstBidder;
   }

   public enum RoundState
   {
      None,
      Started,
   }

   public GameId GameId { get; }

   public PlayerId FirstBidder { get; }

   public RoundState State { get; set; }

   public static Round StartNew(GameId gameId, PlayerId playerId)
   {
      Round round = new (gameId, playerId);
      round.Apply(new RoundStartedEvent(gameId));
      return round;
   }

#pragma warning disable SA1119 // Statement should not use unnecessary parenthesis
   protected override void EnsureValidState()
      => (this.State switch
      {
         RoundState.None => (Action)(() => this.Validate()),
         RoundState.Started => () => this.Validate(),
         _ => () => { },
      }).Invoke();

   protected override void When(DomainEventBase @event)
      => (@event switch
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent()),
         _ => () => { },
      }).Invoke();
#pragma warning restore SA1119 // Statement should not use unnecessary parenthesis

   private void HandleStartedEvent()
   {
      this.State = RoundState.Started;
   }

   private void Validate()
   {
      if (this.State != RoundState.Started)
      {
         throw new InvalidEntityStateException();
      }
   }
}