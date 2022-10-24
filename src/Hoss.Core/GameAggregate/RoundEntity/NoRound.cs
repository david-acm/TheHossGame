// 🃏 The HossGame 🃏
// <copyright file="NoRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.GameAggregate.RoundEntity.Events;
using Hoss.Core.PlayerAggregate;

#endregion

public sealed class NoRound : Round
{
   public NoRound()
      : base(new RoundId(), _ => { })
   {
      this.Apply(new RoundStartedEvent(new NoGameId(), new RoundId(), new List<RoundPlayer>()));
   }

   internal override RoundState State => RoundState.None;

   internal override IReadOnlyList<PlayerDeal> PlayerDeals => new List<PlayerDeal>();

   internal override IReadOnlyList<RoundPlayer> RoundPlayers => new List<RoundPlayer>();

   internal override IReadOnlyList<Bid> Bids => new List<Bid>();

   internal override PlayerId CurrentPlayerId => new NoPlayerId();

   internal override CardSuit SelectedTrump => CardSuit.None;
}
