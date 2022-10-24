// 🃏 The HossGame 🃏
// <copyright file="ARound.Validation.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

   #region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;

#endregion

public sealed partial class ARound
{
   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         RoundState.ShufflingCards => this.ValidateGameStarted(),
         RoundState.DealingCards => this.ValidateCardsShuffled(),
         RoundState.Bidding => this.ValidateCardsDealt(),
         RoundState.SelectingTrump => this.ValidateBiddingFinished(),
         RoundState.PlayingCards => this.ValidateTrumpSelected() && this.ValidateOrderOfPlay(this.tableCenter) && this.ValidateCardPlayed(),
         RoundState.None => false,
         _ => false,
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   private bool ValidateGameStarted()
   {
      return this.RoundPlayers.Count == 4;
   }

   private bool ValidateCardsShuffled()
   {
      return this.ValidateGameStarted() && this.Deals.All(p => p.Cards.Count == 6);
   }

   private bool ValidateCardsDealt()
   {
      return this.ValidateCardsShuffled() && this.Deals.Count == 4 && this.ValidateBidValue() && this.ValidateOrderOfPlay(this.Bids);
   }

   private bool ValidateBiddingFinished()
   {
      return this.ValidateGameStarted() && this.ValidateCardsShuffled() && this.CurrentPlayerId == this.WinningBid().PlayerId;
   }

   private bool ValidateOrderOfPlay(IReadOnlyList<Play> plays)
   {
      bool AnyPlay()
      {
         return plays.Any();
      }

      PlayerId PlayerThatPlayed()
      {
         return plays[^1].PlayerId;
      }

      PlayerId PlayerInTurn() => this.RoundPlayers[^1].PlayerId;

      return !AnyPlay() || (PlayerInTurn() == PlayerThatPlayed());
   }

   private bool ValidateCardPlayed()
   {
      bool NoCardsPlayedYet()
      {
         return !this.tableCenter.Any();
      }

      bool IsFirstCard()
      {
         return this.tableCenter.Count == 1;
      }

      bool PlayerOwnsCard()
      {
         return NoCardsPlayedYet() || !OtherPlayerCards().Union(PreviousTableCenterCards()).Contains(CardPlayed());
      }

      bool PlayedCardFollowsSuit()
      {
         return NoCardsPlayedYet() || IsFirstCard() || this.tableCenter[^2].Card.Suit == CardPlay().Card.Suit;
      }

      CardPlay CardPlay()
      {
         return this.tableCenter[^1];
      }

      Card CardPlayed()
      {
         return CardPlay().Card;
      }

      IEnumerable<Card> OtherPlayerCards()
      {
         return this.Deals.Where(p => p.PlayerId != CardPlay().PlayerId).SelectMany(d => d.Cards);
      }

      IEnumerable<Card> PreviousTableCenterCards()
      {
         return this.tableCenter.Select(t => t.Card).Take(this.tableCenter.Count - 1);
      }

      return PlayerOwnsCard() && PlayedCardFollowsSuit();
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
      var playerSelectedTrump = this.trumpSelection.PlayerId;
      return this.WinningBid().PlayerId == playerSelectedTrump;
   }
}
