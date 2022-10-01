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
   private readonly List<PlayerId> roundPlayers;
   private List<PlayerCards> playerCards = new ();

   private Round(GameId gameId, IEnumerable<PlayerId> playerIds)
      : base(new RoundId())
   {
      this.GameId = gameId;
      this.Deck = NoDeck.New;
      this.roundPlayers = playerIds.ToList();
   }

   public enum RoundState
   {
      None,
      Started,
   }

   public GameId GameId { get; }

   public RoundState State { get; private set; }

   public Deck Deck { get; private set; }

   public IReadOnlyList<PlayerCards> PlayerCards => this.playerCards.AsReadOnly();

   public IReadOnlyList<PlayerId> RoundPlayers => this.roundPlayers.AsReadOnly();

   public static Round StartNew(GameId gameId, IEnumerable<PlayerId> playerIds, IShufflingService shufflingService)
   {
      var round = new Round(gameId, playerIds);
      var shuffledDeck = ADeck.FromShuffling(shufflingService);
      List<PlayerCards> cards = DealCards(shuffledDeck, playerIds);
      round.Apply(new RoundStartedEvent(
         gameId,
         round.Id,
         shuffledDeck,
         cards));

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

   private static List<PlayerCards> DealCards(
      ADeck deck,
      IEnumerable<PlayerId> playerIds)
   {
      var playerHand = playerIds.Select(p => new PlayerCards(p)).ToList();

      while (deck.HasCards)
      {
         playerHand.ForEach(p => p.ReceibeCard(deck.Deal()));
      }

      return playerHand;
   }

   private void HandleStartedEvent(RoundStartedEvent e)
   {
      this.State = RoundState.Started;
      this.Deck = e.Deck;
      this.playerCards = e.PlayerCards.ToList();
   }

   private void Validate()
   {
      if (this.State != RoundState.Started)
      {
         throw new InvalidEntityStateException();
      }
   }
}