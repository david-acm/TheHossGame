// 🃏 The HossGame 🃏
// <copyright file="NoRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public sealed class NoRound : Round
{
   public NoRound()
      : base(new RoundId(), _ => { })
   {
   }

   internal override RoundState State => RoundState.None;

   internal override IReadOnlyList<PlayerDeal> PlayerDeals => new List<PlayerDeal>();

   internal override IReadOnlyList<RoundPlayer> TeamPlayers => new List<RoundPlayer>();

   internal override IReadOnlyList<Bid> Bids => new List<Bid>();

   internal override PlayerId CurrentPlayerId => new NoPlayerId();

   internal override CardSuit SelectedTrump => throw new NotImplementedException();

   protected override bool IsNull => true;

   internal override void Bid(PlayerId playerId, BidValue value)
   {
   }

   internal override void SelectTrump(PlayerId currentPlayerId, CardSuit suit)
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}