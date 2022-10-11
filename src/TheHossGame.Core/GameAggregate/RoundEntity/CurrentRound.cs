// 🃏 The HossGame 🃏
// <copyright file="CurrentRound.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using System.Collections.Generic;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record CurrentRound : ValueObject
{
   private readonly Round currentRound;

   public CurrentRound(Round currentRound)
   {
      this.currentRound = currentRound;
   }

   public IReadOnlyList<PlayerDeal> PlayerDeals => this.currentRound.PlayerDeals;

   internal PlayerId CurrentPlayerId => this.currentRound.CurrentPlayerId;

   public RoundId Id => this.currentRound.Id;

   public Round.RoundState State => this.currentRound.State;

   public IReadOnlyList<Bid> Bids => this.currentRound.Bids;

   public IReadOnlyList<RoundPlayer> TeamPlayers => this.currentRound.TeamPlayers;

   public CardSuit TrumpSelected => this.currentRound.SelectedTrump;
}