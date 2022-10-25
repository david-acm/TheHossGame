// 🃏 The HossGame 🃏
// <copyright file="ARound.Validation.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

   #region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.GameAggregate.RoundEntity.Events;
using Hoss.Core.PlayerAggregate;
using static Hoss.Core.GameAggregate.RoundEntity.BidEntity.BidValue;

#endregion

public sealed partial class ARound
{
   private void EnsurePreconditions(RoundEventBase @event)
   {
#pragma warning disable CS8509
      var valid = @event switch
#pragma warning restore CS8509
      {
         CardPlayedEvent e => this.ValidateCardPlayed(e),
         TrumpSelectedEvent e => this.ValidateTrumpSelected(e),
         BidEvent e => this.ValidateBid(e),
         PlayerCardsDealtEvent e => ValidateCardsDealt(e),
         RoundStartedEvent e => ValidateRoundStarted(e),
         _ => true,
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   private static bool ValidateCardsDealt(PlayerCardsDealtEvent e) => e.Deal.Cards.Count == 6;

   private static bool ValidateRoundStarted(RoundStartedEvent e) => e.TeamPlayers.Count() == 4;

   private bool ValidateTrumpSelected(TrumpSelectedEvent @event) => this.BidWinner == @event.PlayerId;

   private bool ValidateCardPlayed(CardPlayedEvent e) => this.IsThePlayersTurn(e.PlayerId) && this.PlayerHasThatCard(e) && (this.IsOpeningCard() || this.CardFollowsSuit(e) || this.PlayerHasNoCardsOfAskedSuit(e));

   private bool IsOpeningCard() => this.tableCenter.Count == 0;

   private bool CardFollowsSuit(CardPlayedEvent e) => e.Card.Suit == this.AskedSuit();

   private Suit AskedSuit() => this.tableCenter.Last().Card.Suit;

   private bool PlayerHasNoCardsOfAskedSuit(CardPlayedEvent e) => this.CardsForPlayer(e.PlayerId).All(c => c.Suit != this.AskedSuit());

   private bool PlayerHasThatCard(CardPlayedEvent e) => this.CardsForPlayer(e.PlayerId).Contains(e.Card);

   private bool IsThePlayersTurn(PlayerId playerId) => playerId == this.CurrentPlayerId;

   private bool ValidateBid(BidEvent e) => this.ValidateBid(e.Bid) && this.IsThePlayersTurn(e.Bid.PlayerId);

   private bool ValidateBid(Bid bid) => this.ValidateBidValue(bid);

   private bool ValidateBidValue(Bid newBid)
   {
      bool BiggerThanPrevious() => this.bids.TrueForAll(bid => newBid > bid);

      return newBid == Pass || BiggerThanPrevious();
   }
}
