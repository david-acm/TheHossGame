// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
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
      : base(id, when)
   {
   }

   internal abstract RoundState State { get; }

   internal abstract IReadOnlyList<PlayerDeal> PlayerDeals { get; }

   internal abstract IReadOnlyList<RoundPlayer> RoundPlayers { get; }

   internal abstract IReadOnlyList<Bid> Bids { get; }

   internal abstract PlayerId CurrentPlayerId { get; }

   internal abstract CardSuit SelectedTrump { get; }

   internal virtual void Bid(PlayerId playerId, BidValue value)
   {
   }

   internal virtual void SelectTrump(PlayerId playerId, CardSuit suit)
   {
   }

   internal virtual void PlayCard(PlayerId playerId, Card card)
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}
