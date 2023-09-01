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
            TrickPlayedEvent e => this.EnoughTricksHaveBeenPlayed(),
            _ => new ValidationResult(true, string.Empty),
        };

        if (!valid) throw new InvalidEntityStateException(valid.Reason);
    }

    private ValidationResult EnoughTricksHaveBeenPlayed()
    {
        return new ValidationResult(this.tableCenter.CardPlays.Count == this.teamPlayers.Count,
            nameof(this.EnoughTricksHaveBeenPlayed));
    }

    private static ValidationResult ValidateCardsDealt(PlayerCardsDealtEvent e)
    {
        return new ValidationResult(e.Deal.Cards.Count == 6,
            nameof(ValidateCardsDealt));
    }

    private static ValidationResult ValidateRoundStarted(RoundStartedEvent e)
    {
        return new ValidationResult(e.TeamPlayers.Count() == 4,
            nameof(ValidateRoundStarted));
    }

    private ValidationResult ValidateTrumpSelected(TrumpSelectedEvent @event)
    {
        return new ValidationResult(this.BidWinner == @event.PlayerId
                                    && this.Stage == RoundStage.SelectingTrump,
            nameof(this.ValidateTrumpSelected));
    }

    private ValidationResult ValidateCardPlayed(CardPlayedEvent e)
    {
        return new ValidationResult(
            this.IsThePlayersTurn(e.PlayerId) &&
            this.PlayerHasThatCard(e) && (this.IsOpeningCard() ||
                                          this.CardFollowsSuit(e.Card) ||
                                          this.PlayerHasNoCardsOfAskedSuit(e)),
            nameof(this.ValidateCardPlayed));
    }

    private ValidationResult IsOpeningCard()
    {
        return new ValidationResult(this.tableCenter.CardPlays.Count == 0,
            nameof(this.IsOpeningCard));
    }

    private ValidationResult CardFollowsSuit(Card card)
    {
        var comparer = new Suit.SuitComparer(this.SelectedTrump);
        return new ValidationResult(comparer.Equals(card, this.AskedCard()),
            nameof(this.CardFollowsSuit));
    }

    private Card AskedCard()
    {
        return this.tableCenter.CardPlays.Last().Card;
    }

    private ValidationResult PlayerHasNoCardsOfAskedSuit(CardPlayedEvent e)
    {
        return new ValidationResult(
            this.CardsForPlayer(e.PlayerId).All(c => !this.CardFollowsSuit(c)),
            nameof(this.PlayerHasNoCardsOfAskedSuit));
    }

    private ValidationResult PlayerHasThatCard(CardPlayedEvent e)
    {
        return new ValidationResult(
            this.CardsForPlayer(e.PlayerId).Contains(e.Card),
            nameof(this.PlayerHasThatCard));
    }

    private ValidationResult IsThePlayersTurn(PlayerId playerId)
    {
        return new ValidationResult(playerId == this.CurrentPlayerId, nameof(this.IsThePlayersTurn));
    }

    private ValidationResult ValidateBid(BidEvent e)
    {
        return new ValidationResult(this.ValidateBid(e.Bid) && this.IsThePlayersTurn(e.Bid.PlayerId),
            nameof(ValidateBid));
    }

    private bool ValidateBid(Bid bid)
    {
        return this.PlayersCanBid() && this.ValidateBidValue(bid);
    }

    private ValidationResult PlayersCanBid()
    {
        return new ValidationResult(this.Stage == RoundStage.Bidding, nameof(this.PlayersCanBid));
    }

    private ValidationResult ValidateBidValue(Bid newBid)
    {
        bool BiggerThanPrevious()
        {
            return this.bids.TrueForAll(bid => newBid > bid);
        }

        return new ValidationResult(newBid == Pass || BiggerThanPrevious(), nameof(this.ValidateBidValue));
    }
}

public class ValidationResult
{
    private readonly bool isValid;

    public ValidationResult(bool isValid, string reason)
    {
        this.isValid = isValid;
        this.Reason = reason;
    }

    public ValidationResult(ValidationResult[] args)
    {
        this.Reason = string.Concat(args.Select(v => v.Reason), "👉 ");
    }

    public string Reason { get; }

    public static implicit operator bool(ValidationResult result)
    {
        return result.isValid;
    }
}