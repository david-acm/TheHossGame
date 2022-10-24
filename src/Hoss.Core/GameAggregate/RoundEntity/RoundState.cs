// 🃏 The HossGame 🃏
// <copyright file="CurrentRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;

#endregion

public record RoundState(Round currentRound) : ValueObject
{
   private readonly Round currentRound = currentRound;

   public IReadOnlyList<PlayerDeal> PlayerDeals => this.currentRound.PlayerDeals;

   internal PlayerId CurrentPlayerId => this.currentRound.CurrentPlayerId;

   public RoundId Id => this.currentRound.Id;

   public Round.RoundState State => this.currentRound.State;

   public IReadOnlyList<Bid> Bids => this.currentRound.Bids;

   public IReadOnlyList<RoundPlayer> RoundPlayers => this.currentRound.RoundPlayers;

   public CardSuit TrumpSelected => this.currentRound.SelectedTrump;
}
