// 🃏 The HossGame 🃏
// <copyright file="ARound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.GameAggregate.RoundEntity.Events;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static TheHossGame.Core.GameAggregate.Game;

public class ARound : Round
{
   private List<Bid> bids = new ();
   private List<PlayerDeal> deals = new ();
   private Queue<RoundPlayer> teamPlayers = new ();
   private RoundState state;

   private ARound(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Action<DomainEventBase> when)
      : this(gameId, new RoundId(), when)
   {
      this.OrderPlayers(teamPlayers);
   }

   private ARound(GameId gameId, RoundId roundId, Action<DomainEventBase> when)
      : base(roundId, when)
   {
      this.GameId = gameId;
   }

   public override GameId GameId { get; }

   public override RoundState State => this.state;

   public override IReadOnlyList<PlayerDeal> PlayerDeals => this.deals.AsReadOnly();

   public override IReadOnlyList<RoundPlayer> TeamPlayers => this.teamPlayers.ToList().AsReadOnly();

   public override IReadOnlyList<Bid> Bids => this.bids.AsReadOnly();

   public override PlayerId CurrentPlayerId => this.teamPlayers.Peek().PlayerId;

   public override bool IsNull => false;

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

   internal override void Bid(BidCommand bid)
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

   private void HandleCardsDealtEvent() => this.state = RoundState.CardsDealt;

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
