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
            TrickPlayedEvent e => this.tableCenter.CardPlays.Count == 4,
            _ => true,
        };

        if (!valid) throw new InvalidEntityStateException();
    }

    private static bool ValidateCardsDealt(PlayerCardsDealtEvent e)
    {
        return e.Deal.Cards.Count == 6;
    }

    private static bool ValidateRoundStarted(RoundStartedEvent e)
    {
        return e.TeamPlayers.Count() == 4;
    }

    private bool ValidateTrumpSelected(TrumpSelectedEvent @event)
    {
        return this.BidWinner == @event.PlayerId;
    }

    private bool ValidateCardPlayed(CardPlayedEvent e)
    {
        return this.IsThePlayersTurn(e.PlayerId) &&
               this.PlayerHasThatCard(e) && (this.IsOpeningCard() ||
                                             this.CardFollowsSuit(e.Card) ||
                                             this.PlayerHasNoCardsOfAskedSuit(e));
    }

    private bool IsOpeningCard()
    {
        return this.tableCenter.CardPlays.Count == 0;
    }

    private bool CardFollowsSuit(Card card)
    {
        var comparer = new Suit.SuitComparer(this.SelectedTrump);
        return comparer.Equals(card, this.AskedCard());
    }

    private Card AskedCard()
    {
        return this.tableCenter.CardPlays.Last().Card;
    }

    private bool PlayerHasNoCardsOfAskedSuit(CardPlayedEvent e)
    {
        return this.CardsForPlayer(e.PlayerId).All(c => !this.CardFollowsSuit(c));
    }

    private bool PlayerHasThatCard(CardPlayedEvent e)
    {
        return this.CardsForPlayer(e.PlayerId).Contains(e.Card);
    }

    private bool IsThePlayersTurn(PlayerId playerId)
    {
        return playerId == this.CurrentPlayerId;
    }

    private bool ValidateBid(BidEvent e)
    {
        return this.ValidateBid(e.Bid) && this.IsThePlayersTurn(e.Bid.PlayerId);
    }

    private bool ValidateBid(Bid bid)
    {
        return this.PlayersCanBid() && this.ValidateBidValue(bid);
    }

    private bool PlayersCanBid()
    {
        return this.Stage == RoundState.Bidding;
    }

    private bool ValidateBidValue(Bid newBid)
    {
        bool BiggerThanPrevious()
        {
            return this.bids.TrueForAll(bid => newBid > bid);
        }

        return newBid == Pass || BiggerThanPrevious();
    }
}