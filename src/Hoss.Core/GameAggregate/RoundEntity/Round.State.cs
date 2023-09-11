// 🃏 The HossGame 🃏
// <copyright file="ARound.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
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
        : this(gameId, new RoundId(), when)
    {
        this.OrderPlayers(teamPlayers, roundNumber);
    }

    private Round(GameId gameId, RoundId roundId, Action<DomainEventBase> when)
        : base(roundId, when)
    {
        this.GameId = gameId;
    }

    internal override RoundStage Stage => this.stage;

    internal override IReadOnlyList<ADeal> Deals => this.deals.AsReadOnly();

    internal override IReadOnlyList<RoundPlayer> RoundPlayers => this.teamPlayers.ToList().AsReadOnly();

    internal override IReadOnlyList<Bid> Bids => this.bids.AsReadOnly();

    /// <inheritdoc />
    internal override IReadOnlyList<CardPlay> CardsPlayed => this.tableCenter.CardPlays.ToList().AsReadOnly();

    internal override PlayerId CurrentPlayerId => this.teamPlayers.Peek().PlayerId;

    internal override Suit SelectedTrump => this.trumpSelection.Suit;

    private GameId GameId { get; }

    internal override ADeal DealForPlayer(PlayerId playerId)
    {
        return this.deals.First(d => d.PlayerId == playerId);
    }

    internal override IEnumerable<Card> CardsForPlayer(PlayerId playerId)
    {
        return this.deals.First(d => d.PlayerId == playerId).Cards;
    }

    /// <inheritdoc />
    protected override void Apply(DomainEventBase @event)
    {
        var roundEvent = @event as RoundEventBase;
        this.EnsurePreconditions(roundEvent!);
        base.Apply(@event);
    }
}

internal record Trick
{
    private readonly TrumpSelection trumpSelection;

    private Trick(Stack<CardPlay> plays, TrumpSelection trumpSelection)
    {
        this.trumpSelection = trumpSelection;
        this.Plays = plays;
    }

    private Stack<CardPlay> Plays { get; }

    public PlayerId Winner =>
        this.Plays.OrderByDescending(
            c => c.Card,
            new Card.CardComparer(this.trumpSelection.Suit, this.Plays.Last().Card.Suit)).First().PlayerId;

    public static Trick FromTableCenter(TableCenter tableCenter, TrumpSelection trumpSelection)
    {
        return new Trick(tableCenter.CardPlays, trumpSelection);
    }
}

internal record TableCenter
{
    internal TableCenter(Stack<CardPlay>? cardPlays = null)
    {
        this.CardPlays = cardPlays ?? new Stack<CardPlay>();
    }

    internal Stack<CardPlay> CardPlays { get; }

    public TableCenter Push(CardPlay cardPlay)
    {
        var plays = new Stack<CardPlay>(this.CardPlays.Reverse());
        plays.Push(cardPlay);
        return new TableCenter(plays);
    }

    internal static TableCenter Clear()
    {
        return new TableCenter();
    }
}