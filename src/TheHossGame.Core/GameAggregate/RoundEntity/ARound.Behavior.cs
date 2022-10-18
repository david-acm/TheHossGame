// 🃏 The HossGame 🃏
// <copyright file="ARound.Behavior.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static Game;

/// <summary>
///    The behaviour side.
/// </summary>
public sealed partial class ARound
{
   internal static ARound StartNew(
      GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Deck shuffledDeck, Action<DomainEventBase> when)
   {
      var roundPlayers = teamPlayers.ToList();
      var round = new ARound(
         gameId,
         roundPlayers,
         when);
      var playerDeals = DealCards(
         shuffledDeck,
         roundPlayers);
      round.Apply(
         new RoundStartedEvent(
            gameId,
            round.Id,
            round.teamPlayers));
      playerDeals.ForEach(
         cards => round
            .Apply(
               new PlayerCardsDealtEvent(
                  gameId,
                  round.Id,
                  cards)));
      round.Apply(
         new AllCardsDealtEvent(
            gameId,
            round.Id));

      return round;
   }

   internal static ARound FromStream(GameStartedEvent @event, Action<DomainEventBase> when)
   {
      return new ARound(
         @event.GameId,
         @event.RoundId,
         when)
      {
         teamPlayers = new Queue<RoundPlayer>(@event.TeamPlayers),
         deals = @event.Deals.ToList(),
         bids = @event.Bids.ToList(),
         state = RoundState.CardsDealt,
      };
   }

   internal override void Bid(PlayerId playerId, BidValue value)
   {
      this.Apply(
         new BidEvent(
            this.GameId,
            this.Id,
            new Bid(
               playerId,
               value)));

      if (this.bids.Count == 4)
      {
         this.Apply(
            new BidCompleteEvent(
               this.GameId,
               this.Id,
               this.WinningBid()));
      }
   }

   internal override void SelectTrump(PlayerId playerId, CardSuit suit)
   {
      this.Apply(
         new TrumpSelectedEvent(
            this.GameId,
            this.Id,
            playerId,
            suit));
   }

   protected override void When(DomainEventBase @event)
   {
      (@event switch
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent(e)),
         PlayerCardsDealtEvent e => () => this.HandlePlayerCardsDealtEvent(e),
         AllCardsDealtEvent => this.HandleCardsDealtEvent,
         BidEvent e => () => this.HandleBidEvent(e),
         BidCompleteEvent e => () => this.HandleBidCompleteEvent(e),
         TrumpSelectedEvent e => () => this.HandleTrumpSelectedEvent(e),
         _ => () => { },
      }).Invoke();
   }

   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         RoundState.Started => this.ValidateStarted(),
         RoundState.CardsShuffled => this.ValidateCardsShuffled(),
         RoundState.CardsDealt => this.ValidateAllCardsDealt(),
         RoundState.BidFinished => this.ValidateBidFinished(),
         RoundState.TrumpSelected => this.ValidateTrumpSelected(),
         RoundState.None => false,
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
      this.state = RoundState.Started;
      this.teamPlayers = new Queue<RoundPlayer>(e.TeamPlayers.ToList());
   }

   private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
   {
      this.state = RoundState.CardsShuffled;
      this.deals.Add(e.PlayerCards);
   }

   private void HandleBidEvent(BidEvent e)
   {
      this.bids.Add(e.Bid);
      this.FinishTurn();
   }

   private void HandleBidCompleteEvent(BidCompleteEvent e)
   {
      while (this.CurrentPlayerId != e.WinningBid.PlayerId) this.FinishTurn();

      this.state = RoundState.BidFinished;
   }

   private void HandleTrumpSelectedEvent(TrumpSelectedEvent e)
   {
      this.trumpSelection = (e.Trump, e.PlayerId);
      this.state = RoundState.TrumpSelected;
   }

   private void FinishTurn()
   {
      this.teamPlayers.Enqueue(this.teamPlayers.Dequeue());
   }

   private bool ValidateStarted()
   {
      return this.TeamPlayers.Count == 4;
   }

   private bool ValidateCardsShuffled()
   {
      return this.ValidateStarted() && this.PlayerDeals.All(p => p.Cards.Count == 6);
   }

   private bool ValidateAllCardsDealt()
   {
      return this.ValidateCardsShuffled() &&
         this.PlayerDeals.Count == 4 &&
         this.ValidateBids() &&
         this.ValidatePlayerOrder();
   }

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
      return this.ValidateStarted() &&
         this.ValidateCardsShuffled() &&
         this.CurrentPlayerId == this.WinningBid().PlayerId;
   }

   private bool BidPerformedByPlayerInTurn()
   {
      var lastBidIndex = this.Bids.Count - 1;
      var lastPlayerIndex = this.TeamPlayers.Count - 1;

      var lastTurnPlayerId = this.TeamPlayers[lastPlayerIndex].PlayerId;
      var lastBidPlayerId = this.Bids[lastBidIndex].PlayerId;

      var lastBidPlayerWasPlayerInTurn = lastTurnPlayerId == lastBidPlayerId;
      return lastBidPlayerWasPlayerInTurn;
   }

   private bool ValidateBids()
   {
      var orderedBids = this.Bids
                            .Select(
                               (b, i) => new
                               {
                                  Bid = b,
                                  Index = i,
                               })
                            .OrderBy(p => p.Index)
                            .ToList();

      return orderedBids.TrueForAll(
         c =>
         {
            var indexOfPreviousBid = c.Index - 1 > 0 ? c.Index - 1 : 0;
            var lastBid = this.Bids[indexOfPreviousBid].Value;

            var isSameBid = c.Index == indexOfPreviousBid;
            var bidIsBiggerThanLast = c.Bid.Value > lastBid;
            var bidIsAPass = c.Bid.Value == BidValue.Pass;

            return isSameBid || bidIsBiggerThanLast || bidIsAPass;
         });
   }

   private bool ValidateTrumpSelected()
   {
      var playerSelectedTrump = this.trumpSelection.Item2;
      return this.WinningBid().PlayerId == playerSelectedTrump;
   }
}
