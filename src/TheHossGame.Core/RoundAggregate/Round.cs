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
   private readonly List<PlayerDeal> playerCards = new ();
   private List<PlayerId> roundPlayers;

   private Round(GameId gameId, IEnumerable<PlayerId> playerIds)
      : base(new RoundId())
   {
      this.GameId = gameId;
      this.roundPlayers = playerIds.ToList();
   }

   public enum RoundState
   {
      None,
      Started,
      CardsShuffled,
      CardsDealt,
   }

   public GameId GameId { get; }

   public RoundState State { get; private set; }

   public IReadOnlyList<PlayerDeal> PlayerDeals => this.playerCards.AsReadOnly();

   public IReadOnlyList<PlayerId> RoundPlayers => this.roundPlayers.AsReadOnly();

   public static Round StartNew(GameId gameId, IEnumerable<PlayerId> playerIds, IShufflingService shufflingService)
   {
      var round = new Round(gameId, playerIds);
      var shuffledDeck = ADeck.FromShuffling(shufflingService);
      List<PlayerDeal> playerDeals = DealCards(shuffledDeck, playerIds);
      round
         .Apply(new RoundStartedEvent(gameId, round.Id, playerIds));
      playerDeals.ForEach(cards => round
         .Apply(new PlayerCardsDealtEvent(gameId, round.Id, cards)));
      round
         .Apply(new AllCardsDealtEvent(gameId, round.Id));

      return round;
   }

   protected override void When(DomainEventBase @event)
      => (@event switch
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent(e)),
         PlayerCardsDealtEvent e => () => this.HandlePlayerCardsDealtEvent(e),
         AllCardsDealtEvent e => () => this.HandleCardsDealtEvent(),
         _ => () => { },
      }).Invoke();

   protected override void EnsureValidState()
   {
      bool valid = this.State switch
      {
         RoundState.None => false,
         RoundState.Started => this.ValidateStarting(),
         RoundState.CardsShuffled => this.ValidatePlayerCardsDealt(),
         RoundState.CardsDealt => this.ValidateAllCardsDealt(),
         _ => throw new InvalidEntityStateException(),
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   private static List<PlayerDeal> DealCards(
      ADeck deck,
      IEnumerable<PlayerId> playerIds)
   {
      var playerHand = playerIds.Select(p => new PlayerDeal(p)).ToList();

      while (deck.HasCards)
      {
         playerHand.ForEach(p => p.ReceibeCard(deck.Deal()));
      }

      return playerHand;
   }

   private void HandleStartedEvent(RoundStartedEvent e)
   {
      this.State = RoundState.Started;
      this.roundPlayers = e.PlayerIds.ToList();
   }

   private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
   {
      this.State = RoundState.CardsShuffled;
      this.playerCards.Add(e.playerCards);
   }

   private void HandleCardsDealtEvent() => this.State = RoundState.CardsDealt;

   private bool ValidateStarting() => this.RoundPlayers.Count == 4;

   private bool ValidatePlayerCardsDealt() => this.PlayerDeals.All(p => p.Cards.Count == 6);

   private bool ValidateAllCardsDealt() => this.ValidatePlayerCardsDealt() && this.PlayerDeals.Count == 4;
}
