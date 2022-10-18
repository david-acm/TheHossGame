﻿// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public abstract class Round : EntityBase<RoundId>
{
   #region RoundState enum

   public enum RoundState
   {
      None,
      Started,
      CardsShuffled,
      CardsDealt,
      BidFinished,
      TrumpSelected,
   }

   #endregion

   protected Round(RoundId id, Action<DomainEventBase> when)
      : base(
         id,
         when)
   {
   }

   internal abstract RoundState State { get; }

   internal abstract IReadOnlyList<PlayerDeal> PlayerDeals { get; }

   internal abstract IReadOnlyList<RoundPlayer> TeamPlayers { get; }

   internal abstract IReadOnlyList<Bid> Bids { get; }

   internal abstract PlayerId CurrentPlayerId { get; }

   internal abstract CardSuit SelectedTrump { get; }

   internal abstract void Bid(PlayerId playerId, BidValue value);

   internal abstract void SelectTrump(PlayerId playerId, CardSuit suit);
}
