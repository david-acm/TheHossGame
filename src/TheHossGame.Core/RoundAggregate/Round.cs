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
using static TheHossGame.Core.GameAggregate.Game;

public class Round : AggregateRoot<RoundId>
{
   private readonly List<PlayerDeal> playerCards = new ();
   private readonly List<Bid> bids = new ();
   private Queue<TeamPlayer> teamPlayers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
   private Round(GameId gameId, IEnumerable<TeamPlayer> teamPlayers)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
      : base(new RoundId())
   {
      this.GameId = gameId;
      this.OrderPlayers(teamPlayers);
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

   public IReadOnlyList<TeamPlayer> TeamPlayers => this.teamPlayers.ToList().AsReadOnly();

   public IReadOnlyList<Bid> Bids => this.bids.AsReadOnly();

   public PlayerId CurrentPlayerId => this.teamPlayers.Peek().PlayerId;

   public static Round StartNew(GameId gameId, IEnumerable<TeamPlayer> teamPlayers, IShufflingService shufflingService)
   {
      var round = new Round(gameId, teamPlayers);
      var shuffledDeck = ADeck.FromShuffling(shufflingService);
      List<PlayerDeal> playerDeals = DealCards(shuffledDeck, teamPlayers);
      round.Apply(new RoundStartedEvent(gameId, round.Id, round.teamPlayers));
      playerDeals.ForEach(cards => round
         .Apply(new PlayerCardsDealtEvent(gameId, round.Id, cards)));
      round.Apply(new AllCardsDealtEvent(gameId, round.Id));

      return round;
   }

   public void Bid(BidCommand bid)
   {
      this.Apply(new BidEvent(this.GameId, this.Id, new Bid(bid.PlayerId, bid.Value)));
   }

   protected override void When(DomainEventBase @event)
      => (@event switch
      {
         RoundStartedEvent e => (Action)(() => this.HandleStartedEvent(e)),
         PlayerCardsDealtEvent e => () => this.HandlePlayerCardsDealtEvent(e),
         AllCardsDealtEvent e => () => this.HandleCardsDealtEvent(),
         BidEvent e => () => this.HandleBidEvent(e),
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
         _ => throw new InvalidEntityStateException(),
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   private static List<PlayerDeal> DealCards(
      ADeck deck,
      IEnumerable<TeamPlayer> teamPlayers)
   {
      var playerHand = teamPlayers.Select(p => new PlayerDeal(p.PlayerId)).ToList();

      while (deck.HasCards)
      {
         playerHand.ForEach(p => p.ReceibeCard(deck.Deal()));
      }

      return playerHand;
   }

   private void OrderPlayers(IEnumerable<TeamPlayer> teamPlayers)
   {
      var teamPlayerList = teamPlayers.OrderBy(t => t.TeamId).ToList();
      var secondPlayer = teamPlayerList.First(t => t.TeamId == TeamId.Team2);
      var thirdPlayer = teamPlayerList.Last(t => t.TeamId == TeamId.Team1);
      teamPlayerList[2] = thirdPlayer;
      teamPlayerList[1] = secondPlayer;

      this.teamPlayers = new Queue<TeamPlayer>(teamPlayerList);
   }

   private void HandleStartedEvent(RoundStartedEvent e)
   {
      this.State = RoundState.Started;
      this.teamPlayers = new Queue<TeamPlayer>(e.TeamPlayers.ToList());
   }

   private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
   {
      this.State = RoundState.CardsShuffled;
      this.playerCards.Add(e.playerCards);
   }

   private void HandleCardsDealtEvent() => this.State = RoundState.CardsDealt;

   private void HandleBidEvent(BidEvent e)
   {
      this.bids.Add(e.Bid);
      this.teamPlayers.Enqueue(this.teamPlayers.Dequeue());
   }

   private bool ValidateStarted() => this.TeamPlayers.Count == 4;

   private bool ValidateCardsShuffled()
      => this.ValidateStarted() && this.PlayerDeals.All(p => p.Cards.Count == 6);

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
      else
      {
         return true;
      }
   }

   private bool BidPerformedByPlayerInTurn()
   {
      int lastPlayerIndex = this.TeamPlayers.Count - 1;
      int lastBidIndex = this.Bids.Count - 1;

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
}
