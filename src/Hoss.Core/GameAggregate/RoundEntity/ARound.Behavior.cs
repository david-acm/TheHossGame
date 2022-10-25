﻿// 🃏 The HossGame 🃏
// <copyright file="ARound.Behavior.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

   #region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.GameAggregate.RoundEntity.Events;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;
using static Game;

#endregion

/// <summary>
///    The behaviour side.
/// </summary>
public sealed partial class ARound
{
   private PlayerId BidWinner => this.WinningBid().PlayerId;

   internal static ARound StartNew(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Deck shuffledDeck, Action<DomainEventBase> when)
   {
      var roundPlayers = teamPlayers.ToList();
      var round = new ARound(gameId, roundPlayers, when);
      var playerDeals = DealCards(shuffledDeck, roundPlayers);

      round.Apply(new RoundStartedEvent(gameId, round.Id, round.teamPlayers));
      playerDeals.ForEach(cards => round.Apply(new PlayerCardsDealtEvent(gameId, round.Id, cards)));
      round.Apply(new AllCardsDealtEvent(gameId, round.Id));

      return round;
   }

   internal static ARound FromStream(GameStartedEvent @event, Action<DomainEventBase> when)
   {
      return new ARound(@event.GameId, @event.RoundId, when)
      {
         teamPlayers = new Queue<RoundPlayer>(@event.TeamPlayers),
         deals = @event.Deals.ToList(),
         bids = @event.Bids.ToList(),
         state = RoundState.Bidding,
      };
   }

   internal override void Bid(PlayerId playerId, BidValue value)
   {
      this.Apply(new BidEvent(this.GameId, this.Id, new Bid(playerId, value)));

      if (this.bids.Count == 4)
      {
         this.Apply(new BidCompleteEvent(this.GameId, this.Id, this.WinningBid()));
      }
   }

   internal override void SelectTrump(PlayerId playerId, Suit suit)
   {
      this.Apply(new TrumpSelectedEvent(this.GameId, this.Id, playerId, suit));
   }

   internal override void PlayCard(PlayerId playerId, Card card)
   {
      this.Apply(new CardPlayedEvent(this.GameId, this.Id, playerId, card));
   }

   protected override void When(DomainEventBase @event)
   {
      var roundEvent = (RoundEventBase)@event;
#pragma warning disable CS8509
      (roundEvent switch
#pragma warning restore CS8509
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent(e)),
         PlayerCardsDealtEvent e => () => this.HandlePlayerCardsDealtEvent(e),
         AllCardsDealtEvent => this.HandleCardsDealtEvent,
         BidEvent e => () => this.HandleBidEvent(e),
         BidCompleteEvent e => () => this.HandleBidCompleteEvent(e),
         TrumpSelectedEvent e => () => this.HandleTrumpSelectedEvent(e),
         CardPlayedEvent e => () => this.HandleCardPlayedEvent(e),
      }).Invoke();
   }

   private static List<ADeal> DealCards(Deck deck, IEnumerable<RoundPlayer> teamPlayers)
   {
      var playerHand = teamPlayers.Select(p => new ADeal(p.PlayerId)).ToList();

      while (deck.HasCards) playerHand.ForEach(p => p.ReceiveCard(deck.Deal()));

      return playerHand;
   }

   private Bid WinningBid()
   {
      return this.bids.OrderByDescending(b => b.Value).First();
   }

   private void OrderPlayers(IEnumerable<RoundPlayer> players)
   {
      var teamPlayerList = players.OrderBy(t => t.TeamId).ToList();
      var secondPlayer = teamPlayerList.First(t => t.TeamId == TeamId.Team2);
      var thirdPlayer = teamPlayerList.Last(t => t.TeamId == TeamId.Team1);
      teamPlayerList[2] = thirdPlayer;
      teamPlayerList[1] = secondPlayer;

      this.teamPlayers = new Queue<RoundPlayer>(teamPlayerList);
   }

   private void HandleStartedEvent(RoundStartedEvent e)
   {
      this.state = RoundState.ShufflingCards;
      this.teamPlayers = new Queue<RoundPlayer>(e.TeamPlayers.ToList());
   }

   private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
   {
      this.state = RoundState.DealingCards;
      this.deals.Add(e.Deal);
   }

   private void HandleCardsDealtEvent()
   {
      this.state = RoundState.Bidding;
   }

   private void HandleBidEvent(BidEvent e)
   {
      this.Play(() => this.bids.Add(e.Bid));
   }

   private void HandleBidCompleteEvent(BidCompleteEvent e)
   {
      while (this.CurrentPlayerId != e.WinningBid.PlayerId) this.FinishTurn();

      this.state = RoundState.SelectingTrump;
   }

   private void HandleTrumpSelectedEvent(TrumpSelectedEvent e)
   {
      this.trumpSelection = new TrumpSelection(e.PlayerId, e.Trump);
      this.state = RoundState.PlayingCards;
   }

   private void HandleCardPlayedEvent(CardPlayedEvent @event)
   {
      this.Play
         (() =>
         {
            var cardPlay = new CardPlay(@event.PlayerId, @event.Card);
            this.tableCenter.Push(cardPlay);
            this.deals.First(d => d.PlayerId == @event.PlayerId).PlayCard(@event.Card);
         });
   }

   private void Play(Action action)
   {
      action();
      this.FinishTurn();
   }

   private void FinishTurn()
   {
      this.teamPlayers.Enqueue(this.teamPlayers.Dequeue());
   }
}
