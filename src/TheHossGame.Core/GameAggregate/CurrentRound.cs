// 🃏 The HossGame 🃏
// <copyright file="AGame.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using System.Collections.Generic;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.SharedKernel;

public record CurrentRound : ValueObject
{
   private readonly Round currentRound;

   public CurrentRound(Round currentRound)
   {
      this.currentRound = currentRound;
   }

   public IReadOnlyList<PlayerDeal> PlayerDeals => this.currentRound.PlayerDeals;

   public PlayerId CurrentPlayerId => this.currentRound.CurrentPlayerId;

   public RoundId Id => this.currentRound.Id;

   public Round.RoundState State => this.currentRound.State;

   public IReadOnlyList<Bid> Bids => this.currentRound.Bids;

   public IReadOnlyList<TeamPlayer> TeamPlayers => this.currentRound.TeamPlayers;
}