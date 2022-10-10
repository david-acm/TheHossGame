// 🃏 The HossGame 🃏
// <copyright file="NoRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public class NoRound : Round
{
   public NoRound()
      : base(new RoundId(), (DomainEventBase e) => { })
   {
   }

   public override bool IsNull => true;

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