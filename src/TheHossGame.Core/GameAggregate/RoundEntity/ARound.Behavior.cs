// 🃏 The HossGame 🃏
// <copyright file="ARound.Behavior.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using System;
using System.Collections.Generic;
using System.Linq;
using static TheHossGame.Core.GameAggregate.Game;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

/// <summary>
/// The behaviour side.
/// </summary>
public partial class ARound : Round
{
   internal static ARound StartNew(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Deck shuffledDeck, Action<DomainEventBase> when)
   {
      var round = new ARound(gameId, teamPlayers, when);
      List<PlayerDeal> playerDeals = DealCards(shuffledDeck, teamPlayers);
      round.Apply(new RoundStartedEvent(gameId, round.Id, round.teamPlayers));
      playerDeals.ForEach(cards => round
         .Apply(new PlayerCardsDealtEvent(gameId, round.Id, cards)));
      round.Apply(new AllCardsDealtEvent(gameId, round.Id));

      return round;
   }

   internal static ARound FromStream(GameStartedEvent @event, Action<DomainEventBase> when)
      => new (@event.GameId, @event.RoundId, when)
      {
         teamPlayers = new Queue<RoundPlayer>(@event.TeamPlayers),
         deals = @event.Deals.ToList(),
         bids = @event.Bids.ToList(),
         state = RoundState.CardsDealt,
      };

   internal override void Bid(PlayerId playerId, BidValue value)
   {
      this.Apply(new BidEvent(this.GameId, this.Id, new Bid(playerId, value)));

      if (this.bids.Count == 4)
      {
         this.Apply(new BidCompleteEvent(this.GameId, this.Id, this.WinningBid()));
      }
   }

   internal override void SelectTrump(PlayerId playerId, CardSuit suit)
   {
      this.Apply(new TrumpSelectedEvent(this.GameId, this.Id, playerId, suit));
   }

   protected override void When(DomainEventBase @event)
      => (@event switch
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent(e)),
         PlayerCardsDealtEvent e => () => this.HandlePlayerCardsDealtEvent(e),
         AllCardsDealtEvent e => () => this.HandleCardsDealtEvent(),
         BidEvent e => () => this.HandleBidEvent(e),
         BidCompleteEvent e => () => this.HandleBidCompleteEvent(e),
         TrumpSelectedEvent e => () => this.HandleTrumpSelectedEvent(e),
         _ => () => { },
      }).Invoke();

   protected override void EnsureValidState()
   {
      bool valid = this.State switch
      {
         RoundState.None => false,
         RoundState.Started => this.ValidateStarted(),
         RoundState.CardsShuffled => this.ValidateCardsShuffled(),
         RoundState.CardsDealt => this.ValidateAllCardsDealt(),
         RoundState.BidFinished => this.ValidateBidFinished(),
         RoundState.TrumpSelected => this.ValidateTrumpSelected(),
         _ => throw new InvalidEntityStateException(),
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   private static List<PlayerDeal> DealCards(
      Deck deck,
      IEnumerable<RoundPlayer> teamPlayers)
   {
      var playerHand = teamPlayers.Select(p => new PlayerDeal(p.PlayerId)).ToList();

      while (deck.HasCards)
      {
         playerHand.ForEach(p => p.ReceibeCard(deck.Deal()));
      }

      return playerHand;
   }

   private Bid WinningBid()
   {
      return this.bids.OrderByDescending(b => b.Value).First();
   }

   private void OrderPlayers(IEnumerable<RoundPlayer> teamPlayers)
   {
      var teamPlayerList = teamPlayers.OrderBy(t => t.TeamId).ToList();
      var secondPlayer = teamPlayerList.First(t => t.TeamId == TeamId.Team2);
      var thirdPlayer = teamPlayerList.Last(t => t.TeamId == TeamId.Team1);
      teamPlayerList[2] = thirdPlayer;
      teamPlayerList[1] = secondPlayer;

      this.teamPlayers = new Queue<RoundPlayer>(teamPlayerList);
   }

   private void HandleStartedEvent(RoundStartedEvent e)
   {
      this.state = RoundState.Started;
      this.teamPlayers = new Queue<RoundPlayer>(e.TeamPlayers.ToList());
   }

   private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
   {
      this.state = RoundState.CardsShuffled;
      this.deals.Add(e.playerCards);
   }

   private void HandleBidEvent(BidEvent e)
   {
      this.bids.Add(e.Bid);
      this.FinishTurn();
   }

   private void HandleBidCompleteEvent(BidCompleteEvent e)
   {
      while (this.CurrentPlayerId != e.WinningBid.PlayerId)
      {
         this.FinishTurn();
      }

      this.state = RoundState.BidFinished;
   }

   private void HandleTrumpSelectedEvent(TrumpSelectedEvent e)
   {
      this.trumpSelection = (e.Trump, e.playerId);
      this.state = RoundState.TrumpSelected;
   }

   private void FinishTurn()
   {
      this.teamPlayers.Enqueue(this.teamPlayers.Dequeue());
   }

   private bool ValidateStarted() => this.TeamPlayers.Count == 4;

   private bool ValidateCardsShuffled()
      => this.ValidateStarted() && this.PlayerDeals.All(p => p.Cards.Count == 6);

   private bool ValidateAllCardsDealt() => this.ValidateCardsShuffled() &&
         this.PlayerDeals.Count == 4 &&
         this.ValidateBids() &&
         this.ValidatePlayerOrder();

   private bool ValidatePlayerOrder()
   {
      if (this.Bids.Any())
      {
         return this.BidPerformedByPlayerInTurn();
      }

      return true;
   }

   private bool ValidateBidFinished()
   {
      return this.ValidateStarted() && this.ValidateCardsShuffled() && this.CurrentPlayerId == this.WinningBid().PlayerId;
   }

   private bool BidPerformedByPlayerInTurn()
   {
      int lastBidIndex = this.Bids.Count - 1;
      int lastPlayerIndex = this.TeamPlayers.Count - 1;

      var lastTurnPlayerId = this.TeamPlayers[lastPlayerIndex].PlayerId;
      var lastBidPlayerId = this.Bids[lastBidIndex].PlayerId;

      bool lastBidPlayerWasPlayerInTurn = lastTurnPlayerId == lastBidPlayerId;
      return lastBidPlayerWasPlayerInTurn;
   }

   private bool ValidateBids()
   {
      var orderedBids = this.Bids
         .Select((b, i) => new { Bid = b, Index = i })
         .OrderBy(p => p.Index)
         .ToList();

      return orderedBids.TrueForAll(c =>
      {
         int indexOfPreviousBid = c.Index - 1 > 0 ? c.Index - 1 : 0;
         var lastBid = this.Bids[indexOfPreviousBid].Value;

         bool isSameBid = c.Index == indexOfPreviousBid;
         bool bidIsBiggerThanLast = c.Bid.Value > lastBid;
         bool bidIsAPass = c.Bid.Value == BidValue.Pass;

         return isSameBid || bidIsBiggerThanLast || bidIsAPass;
      });
   }

   private bool ValidateTrumpSelected()
   {
      PlayerId playerSelectedTrump = this.trumpSelection.Item2;
      return this.WinningBid().PlayerId == playerSelectedTrump;
   }
}
