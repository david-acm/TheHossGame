// 🃏 The HossGame 🃏
// <copyright file="ARound.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using DeckValueObjects;
using static RoundEvents;

#endregion

/// <summary>
///    The state side.
/// </summary>
public sealed partial class Round : RoundBase
{
    private readonly Stack<Trick> tricks = new();
    private List<Bid> bids = new();
    private List<ADeal> deals = new();
    private RoundStage stage;
    private TableCenter tableCenter = new();
    private Queue<RoundPlayer> teamPlayers = new();
    private TrumpSelection trumpSelection = new(new NoPlayerId(), Suit.None);

    private Round(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Action<DomainEventBase> when,
        int roundNumber = 0)
        : this(gameId, new RoundId(Guid.NewGuid()), when)
    {
        OrderPlayers(teamPlayers, roundNumber);
    }

    private Round(GameId gameId, RoundId roundId, Action<DomainEventBase> when)
        : base(roundId, when)
    {
        GameId = gameId;
    }

    internal override RoundStage Stage => stage;

    internal override IReadOnlyList<ADeal> Deals => deals.AsReadOnly();

    internal override IReadOnlyList<RoundPlayer> RoundPlayers => teamPlayers.ToList().AsReadOnly();

    internal override IReadOnlyList<Bid> Bids => bids.AsReadOnly();

    /// <inheritdoc />
    internal override IReadOnlyList<CardPlay> CardsPlayed => tableCenter.CardPlays.ToList().AsReadOnly();

    internal override PlayerId CurrentPlayerId => teamPlayers.Peek().PlayerId;

    internal override Suit SelectedTrump => trumpSelection.Suit;

    private GameId GameId { get; }

    internal override ADeal DealForPlayer(PlayerId playerId)
    {
        return deals.First(d => d.PlayerId == playerId);
    }

    internal override IEnumerable<Card> CardsForPlayer(PlayerId playerId)
    {
        return deals.First(d => d.PlayerId.Id == playerId.Id).Cards;
    }

    /// <inheritdoc />
    protected override void Apply(DomainEventBase @event)
    {
        var roundEvent = @event as RoundEventBase;
        EnsurePreconditions(roundEvent!);
        base.Apply(@event);
    }
}

internal record Trick
{
    private readonly TrumpSelection trumpSelection;

    private Trick(Stack<CardPlay> plays, TrumpSelection trumpSelection)
    {
        this.trumpSelection = trumpSelection;
        Plays = plays;
    }

    private Stack<CardPlay> Plays { get; }

    public PlayerId Winner =>
        Plays.OrderByDescending(
            c => c.Card,
            new Card.CardComparer(trumpSelection.Suit, Plays.Last().Card.Suit)).First().PlayerId;

    public static Trick FromTableCenter(TableCenter tableCenter, TrumpSelection trumpSelection)
    {
        return new Trick(tableCenter.CardPlays, trumpSelection);
    }
}

internal record TableCenter
{
    internal TableCenter(Stack<CardPlay>? cardPlays = null)
    {
        CardPlays = cardPlays ?? new Stack<CardPlay>();
    }

    internal Stack<CardPlay> CardPlays { get; }

    public TableCenter Push(CardPlay cardPlay)
    {
        var plays = new Stack<CardPlay>(CardPlays.Reverse());
        plays.Push(cardPlay);
        return new TableCenter(plays);
    }

    internal static TableCenter Clear()
    {
        return new TableCenter();
    }
}