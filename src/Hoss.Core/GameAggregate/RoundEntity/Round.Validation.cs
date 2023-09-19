// 🃏 The HossGame 🃏
// <copyright file="ARound.Validation.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using DeckValueObjects;
using static BidValue;
using static RoundEvents;

#endregion

public sealed partial class Round
{
    private void EnsurePreconditions(RoundEventBase @event)
    {
        var valid = @event switch
        {
            CardPlayedEvent e => ValidateCardPlayed(e),
            TrumpSelectedEvent e => ValidateTrumpSelected(e),
            BidEvent e => ValidateBid(e),
            PlayerCardsDealtEvent e => ValidateCardsDealt(e),
            RoundStartedEvent e => ValidateRoundStarted(e),
            TrickPlayedEvent e => EnoughTricksHaveBeenPlayed(),
            _ => new ValidationResult(true, string.Empty),
        };

        if (!valid) throw new InvalidEntityStateException(valid.Reason);
    }

    private ValidationResult EnoughTricksHaveBeenPlayed()
    {
        return new ValidationResult(tableCenter.CardPlays.Count == teamPlayers.Count,
            nameof(EnoughTricksHaveBeenPlayed));
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
        return new ValidationResult(BidWinner == @event.PlayerId
                                    && Stage == RoundStage.SelectingTrump,
            nameof(ValidateTrumpSelected));
    }

    private ValidationResult ValidateCardPlayed(CardPlayedEvent e)
    {
        return new ValidationResult(
            IsThePlayersTurn(e.PlayerId) &&
            PlayerHasThatCard(e) && (IsOpeningCard() ||
                                          CardFollowsSuit(e.Card) ||
                                          PlayerHasNoCardsOfAskedSuit(e)),
            nameof(ValidateCardPlayed));
    }

    private ValidationResult IsOpeningCard()
    {
        return new ValidationResult(tableCenter.CardPlays.Count == 0,
            nameof(IsOpeningCard));
    }

    private ValidationResult CardFollowsSuit(Card card)
    {
        var comparer = new Suit.SuitComparer(SelectedTrump);
        return new ValidationResult(comparer.Equals(card, AskedCard()),
            nameof(CardFollowsSuit));
    }

    private Card AskedCard()
    {
        return tableCenter.CardPlays.Last().Card;
    }

    private ValidationResult PlayerHasNoCardsOfAskedSuit(CardPlayedEvent e)
    {
        return new ValidationResult(
            CardsForPlayer(e.PlayerId).All(c => !CardFollowsSuit(c)),
            nameof(PlayerHasNoCardsOfAskedSuit));
    }

    private ValidationResult PlayerHasThatCard(CardPlayedEvent e)
    {
        return new ValidationResult(
            CardsForPlayer(e.PlayerId).Contains(e.Card),
            nameof(PlayerHasThatCard));
    }

    private ValidationResult IsThePlayersTurn(PlayerId playerId)
    {
        return new ValidationResult(playerId == CurrentPlayerId, nameof(IsThePlayersTurn));
    }

    private ValidationResult ValidateBid(BidEvent e)
    {
        return new ValidationResult(ValidateBid(e.Bid) && IsThePlayersTurn(e.Bid.PlayerId),
            nameof(ValidateBid));
    }

    private bool ValidateBid(Bid bid)
    {
        return PlayersCanBid() && ValidateBidValue(bid);
    }

    private ValidationResult PlayersCanBid()
    {
        return new ValidationResult(Stage == RoundStage.Bidding, nameof(PlayersCanBid));
    }

    private ValidationResult ValidateBidValue(Bid newBid)
    {
        bool BiggerThanPrevious()
        {
            return bids.TrueForAll(bid => newBid > bid);
        }

        return new ValidationResult(newBid == Pass || BiggerThanPrevious(), nameof(ValidateBidValue));
    }
}

public class ValidationResult
{
    private readonly bool isValid;

    public ValidationResult(bool isValid, string reason)
    {
        this.isValid = isValid;
        Reason = reason;
    }

    public ValidationResult(ValidationResult[] args)
    {
        Reason = string.Concat(args.Select(v => v.Reason), "👉 ");
    }

    public string Reason { get; }

    public static implicit operator bool(ValidationResult result)
    {
        return result.isValid;
    }
}