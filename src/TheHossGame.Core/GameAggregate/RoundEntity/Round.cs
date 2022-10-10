// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

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

   public abstract IReadOnlyList<RoundPlayer> TeamPlayers { get; }

   public abstract IReadOnlyList<Bid> Bids { get; }

   public abstract PlayerId CurrentPlayerId { get; }

   internal abstract void Bid(BidCommand bid);
}
