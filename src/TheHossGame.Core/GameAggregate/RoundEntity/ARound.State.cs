// 🃏 The HossGame 🃏
// <copyright file="ARound.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

/// <summary>
///    The state side.
/// </summary>
public sealed partial class ARound : Round
{
   private List<Bid> bids = new ();
   private List<PlayerDeal> deals = new ();
   private RoundState state;
   private Queue<RoundPlayer> teamPlayers = new ();
   private (CardSuit, PlayerId) trumpSelection = (CardSuit.None, new NoPlayerId());

   private ARound(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Action<DomainEventBase> when)
      : this(
         gameId,
         new RoundId(),
         when)
   {
      this.OrderPlayers(teamPlayers);
   }

   private ARound(GameId gameId, RoundId roundId, Action<DomainEventBase> when)
      : base(
         roundId,
         when)
   {
      this.GameId = gameId;
   }

   internal override RoundState State => this.state;

   internal override IReadOnlyList<PlayerDeal> PlayerDeals => this.deals.AsReadOnly();

   internal override IReadOnlyList<RoundPlayer> TeamPlayers => this.teamPlayers.ToList().AsReadOnly();

   internal override IReadOnlyList<Bid> Bids => this.bids.AsReadOnly();

   internal override PlayerId CurrentPlayerId => this.teamPlayers.Peek().PlayerId;

   internal override CardSuit SelectedTrump => this.trumpSelection.Item1;

   protected override bool IsNull => false;

   private GameId GameId { get; }

   private void HandleCardsDealtEvent()
   {
      this.state = RoundState.CardsDealt;
   }
}
