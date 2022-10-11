// 🃏 The HossGame 🃏
// <copyright file="NoRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public class NoRound : Round
{
   public NoRound()
      : base(new RoundId(), (e) => { })
   {
   }

   public override bool IsNull => true;

   internal override GameId GameId => new NoGameId();

   internal override RoundState State => RoundState.None;

   internal override IReadOnlyList<PlayerDeal> PlayerDeals => new List<PlayerDeal>() { };

   internal override IReadOnlyList<RoundPlayer> TeamPlayers => new List<RoundPlayer>() { };

   internal override IReadOnlyList<Bid> Bids => new List<Bid>() { };

   internal override PlayerId CurrentPlayerId => new NoPlayerId();

   internal override CardSuit SelectedTrump => throw new NotImplementedException();

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