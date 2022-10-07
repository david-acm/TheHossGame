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

public class ARound : Round
{
   private readonly List<PlayerDeal> playerCards = new ();
   private readonly List<Bid> bids = new ();
   private Queue<TeamPlayer> teamPlayers;
   private RoundState state;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
   private ARound(GameId gameId, IEnumerable<TeamPlayer> teamPlayers, Action<DomainEventBase> when)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
      : base(new RoundId(), when)
   {
      this.GameId = gameId;
      this.OrderPlayers(teamPlayers);
   }

   public override GameId GameId { get; }

   public override RoundState State => this.state;

   public override IReadOnlyList<PlayerDeal> PlayerDeals => this.playerCards.AsReadOnly();

   public override IReadOnlyList<TeamPlayer> TeamPlayers => this.teamPlayers.ToList().AsReadOnly();

   public override IReadOnlyList<Bid> Bids => this.bids.AsReadOnly();

   public override PlayerId CurrentPlayerId => this.teamPlayers.Peek().PlayerId;

   internal static ARound StartNew(GameId gameId, IEnumerable<TeamPlayer> teamPlayers, IShufflingService shufflingService, Action<DomainEventBase> when)
   {
      var round = new ARound(gameId, teamPlayers, when);
      var shuffledDeck = ADeck.FromShuffling(shufflingService);
      List<PlayerDeal> playerDeals = DealCards(shuffledDeck, teamPlayers);
      round.Apply(new RoundStartedEvent(gameId, round, round.teamPlayers));
      playerDeals.ForEach(cards => round
         .Apply(new PlayerCardsDealtEvent(gameId, round.Id, cards)));
      round.Apply(new AllCardsDealtEvent(gameId, round.Id));

      return round;
   }

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
      this.state = RoundState.Started;
      this.teamPlayers = new Queue<TeamPlayer>(e.TeamPlayers.ToList());
   }

   private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
   {
      this.state = RoundState.CardsShuffled;
      this.playerCards.Add(e.playerCards);
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

public abstract class Round : EntityBase<RoundId>
{
   protected Round(RoundId id, Action<DomainEventBase> when)
      : base(id, when)
   {
   }

   public enum RoundState
   {
      None,
      Started,
      CardsShuffled,
      CardsDealt,
   }

   public abstract GameId GameId { get; }

   public abstract RoundState State { get; }

   public abstract IReadOnlyList<PlayerDeal> PlayerDeals { get; }

   public abstract IReadOnlyList<TeamPlayer> TeamPlayers { get; }

   public abstract IReadOnlyList<Bid> Bids { get; }

   public abstract PlayerId CurrentPlayerId { get; }

   internal abstract void Bid(BidCommand bid);
}

public class NoRound : Round
{
   public NoRound()
      : base(new RoundId(), (DomainEventBase e) => { })
   {
   }

   public override GameId GameId => new NoGameId();

   public override RoundState State => RoundState.None;

   public override IReadOnlyList<PlayerDeal> PlayerDeals => new List<PlayerDeal>() { };

   public override IReadOnlyList<TeamPlayer> TeamPlayers => new List<TeamPlayer>() { };

   public override IReadOnlyList<Bid> Bids => new List<Bid>() { };

   public override PlayerId CurrentPlayerId => new NoPlayerId();

   internal override void Bid(BidCommand bid)
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}