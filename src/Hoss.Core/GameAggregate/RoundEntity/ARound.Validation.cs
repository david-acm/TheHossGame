// 🃏 The HossGame 🃏
// <copyright file="ARound.Validation.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.PlayerAggregate;

#endregion

public sealed partial class ARound
{
   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         RoundState.Started => this.ValidateStarted(),
         RoundState.CardsShuffled => this.ValidateCardsShuffled(),
         RoundState.CardsDealt => this.ValidateAllCardsDealt(),
         RoundState.BidFinished => this.ValidateBidFinished(),
         RoundState.TrumpSelected =>
            this.ValidateTrumpSelected() &&
            this.ValidateOrderOfPlay(this.plays.Select(t => t.PlayerId).ToList()),
         RoundState.None => false,
         _ => throw new InvalidEntityStateException(),
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   private bool ValidateStarted()
   {
      return this.RoundPlayers.Count == 4;
   }

   private bool ValidateCardsShuffled()
   {
      return this.ValidateStarted() && this.PlayerDeals.All(p => p.Cards.Count == 6);
   }

   private bool ValidateAllCardsDealt()
   {
      return this.ValidateCardsShuffled() &&
         this.PlayerDeals.Count == 4 &&
         this.ValidateBidValue() &&
         this.ValidateOrderOfPlay(this.Bids.Select(b => b.PlayerId).ToList());
   }

   private bool ValidateBidFinished()
   {
      return this.ValidateStarted() && this.ValidateCardsShuffled() && this.CurrentPlayerId == this.WinningBid().PlayerId;
   }

   private bool ValidateOrderOfPlay(IReadOnlyList<PlayerId> playerList)
   {
      bool AnyPlay() => playerList.Any();

      PlayerId PlayerThatPlayed() => playerList[^1];
      PlayerId PlayerInTurn() => this.RoundPlayers[^1].PlayerId;

      return !AnyPlay() || (PlayerInTurn() == PlayerThatPlayed());
   }

   private bool ValidateBidValue()
   {
      var orderedBids = this.Bids.Select
         ((b, i) => new
         {
            Bid = b,
            Index = i,
         }).OrderBy(p => p.Index).ToList();

      return orderedBids.TrueForAll
         (c =>
         {
            var indexOfPreviousBid = c.Index - 1 > 0 ? c.Index - 1 : 0;
            var lastBid = this.Bids[indexOfPreviousBid].Value;

            var isSameBid = c.Index == indexOfPreviousBid;
            var bidIsBiggerThanLast = c.Bid.Value > lastBid;
            var bidIsAPass = c.Bid.Value == BidValue.Pass;

            return isSameBid || bidIsBiggerThanLast || bidIsAPass;
         });
   }

   private bool ValidateTrumpSelected()
   {
      var playerSelectedTrump = this.trumpSelection.Item2;
      return this.WinningBid().PlayerId == playerSelectedTrump;
   }
}
