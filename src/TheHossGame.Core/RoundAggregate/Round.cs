// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public class Round : AggregateRoot<RoundId>
{
   private Round(GameId gameId, PlayerId firstBidder)
      : base(new RoundId())
   {
      this.GameId = gameId;
      this.FirstBidder = firstBidder;
      this.Deck = NoDeck.New;
   }

   public enum RoundState
   {
      None,
      Started,
   }

   public GameId GameId { get; }

   public PlayerId FirstBidder { get; }

   public RoundState State { get; private set; }

   public Deck Deck { get; private set; }

   public static Round StartNew(GameId gameId, PlayerId playerId, IShufflingService shufflingService)
   {
      var round = new Round(gameId, playerId);
      var shuffledDeck = ADeck.FromShuffling(shufflingService);
      round.Apply(new RoundStartedEvent(
         gameId,
         round.Id,
         shuffledDeck));

      return round;
   }

   protected override void EnsureValidState()
      => (this.State switch
      {
         RoundState.None => (Action)(() => this.Validate()),
         RoundState.Started => () => this.Validate(),
         _ => () => throw new InvalidEntityStateException(),
      }).Invoke();

   protected override void When(DomainEventBase @event)
      => (@event switch
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent(e)),
         _ => () => { },
      }).Invoke();

   private void HandleStartedEvent(RoundStartedEvent e)
   {
      this.State = RoundState.Started;
      this.Deck = e.Deck;
   }

   private void Validate()
   {
      if (this.State != RoundState.Started)
      {
         throw new InvalidEntityStateException();
      }
   }
}