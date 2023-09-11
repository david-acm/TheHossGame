// 🃏 The HossGame 🃏
// <copyright file="ARound.Behavior.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.GameAggregate.RoundEntity.RoundScoreValueObject;
using static Game;
using static RoundEvents;

#endregion

/// <summary>
///     The behaviour side.
/// </summary>
public sealed partial class Round
{
    private PlayerId BidWinner => this.WinningBid().PlayerId;

    internal static Round StartNew(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Deck shuffledDeck,
        Action<DomainEventBase> when,
        int roundNumber = 0)
    {
        var roundPlayers = teamPlayers.ToList();
        var round = new Round(gameId, roundPlayers, when, roundNumber);
        var playerDeals = DealCards(shuffledDeck, roundPlayers);

        round.Apply(new RoundStartedEvent(gameId, round.Id, round.teamPlayers));
        playerDeals.ForEach(cards => round.Apply(new PlayerCardsDealtEvent(gameId, round.Id, cards)));
        round.Apply(new AllCardsDealtEvent(gameId, round.Id));

        return round;
    }

    internal static Round FromStream(GameEvents.GameStartedEvent @event, Action<DomainEventBase> when)
    {
        return new Round(@event.GameId, @event.RoundId, when)
        {
            teamPlayers = new Queue<RoundPlayer>(@event.TeamPlayers),
            deals = @event.Deals.ToList(),
            bids = @event.Bids.ToList(),
            stage = RoundStage.Bidding,
        };
    }

    internal override void Bid(PlayerId playerId, BidValue value)
    {
        this.Apply(new BidEvent(this.GameId, this.Id, new Bid(playerId, value)));

        if (this.bids.Count == 4 && this.Stage != RoundStage.Hossinng)
            this.Apply(new BidCompleteEvent(this.GameId, this.Id, this.WinningBid()));
    }

    /// <inheritdoc />
    internal override void RequestHoss(PlayerId playerId, Card card)
    {
        this.Apply(new HossRequestedEvent(this.GameId, this.Id, new HossRequest(playerId, card)));
    }

    /// <inheritdoc />
    internal override void GiveHossCard(PlayerId playerId, Card card)
    {
        this.Apply(new PartnerHossCardGivenEvent(this.GameId, this.Id, new HossPartnerCard(playerId, card)));
        this.Apply(new BidEvent(this.GameId, this.Id, new Bid(this.GetHossingPartner(playerId), BidValue.Hoss)));
        this.Apply(new BidCompleteEvent(this.GameId, this.Id, this.WinningBid()));
    }

    internal override void SelectTrump(PlayerId playerId, Suit suit)
    {
        this.Apply(new TrumpSelectedEvent(this.GameId, this.Id, playerId, suit));
    }

    internal override void PlayCard(PlayerId playerId, Card card)
    {
        this.Apply(new CardPlayedEvent(this.GameId, this.Id, playerId, card));
        if (this.CardsPlayed.Count == this.RoundPlayers.Count)
            this.Apply(new TrickPlayedEvent(this.GameId, this.Id, this.GetHandWinner()));

        if (this.tricks.Count != 6) return;
        var bidWinningTeam = this.GetBiddingTeam();
        this.Apply(new RoundPlayedEvent(
            this.GameId,
            this.Id, RoundScore.FromRound(bidWinningTeam, this.tricks, this.WinningBid())));
    }

    private (RoundPlayer, RoundPlayer) GetBiddingTeam()
    {
        var bidWinningPlayerId = this.WinningBid().PlayerId;
        var biddingTeam = this.teamPlayers.FirstOrDefault(t => t.PlayerId == bidWinningPlayerId)?.TeamId ??
                          this.GetHossingTeam(bidWinningPlayerId);
        var bidWinningTeam = (this.teamPlayers.First(t => t.TeamId == biddingTeam),
            this.teamPlayers.Last(t => t.TeamId == biddingTeam));
        return bidWinningTeam;
    }

    private CardPlay GetHandWinner()
    {
        return this.CardsPlayed.OrderByDescending(c => c.Card,
                new Card.CardComparer(this.SelectedTrump, this.CardsPlayed.Last().Card.Suit))
            .First();
    }

    protected override void When(DomainEventBase @event)
    {
        var roundEvent = (RoundEventBase) @event;
#pragma warning disable CS8509
        (roundEvent switch
#pragma warning restore CS8509
        {
            RoundStartedEvent e => (Action) (() => this.HandleStartedEvent(e)),
            PlayerCardsDealtEvent e => () => this.HandlePlayerCardsDealtEvent(e),
            AllCardsDealtEvent => this.HandleCardsDealtEvent,
            BidEvent e => () => this.HandleBidEvent(e),
            HossRequestedEvent e => () => this.HandleRequestHossEvent(e),
            PartnerHossCardGivenEvent e => () => this.HandlePartnerHossCardGiven(e),
            BidCompleteEvent e => () => this.HandleBidCompleteEvent(e),
            TrumpSelectedEvent e => () => this.HandleTrumpSelectedEvent(e),
            CardPlayedEvent e => () => this.HandleCardPlayedEvent(e),
            TrickPlayedEvent e => () => this.HandleHandPlayedEvent(e),
            RoundPlayedEvent e => () => this.HandleRoundPlayedEvent(e),
        }).Invoke();
    }

    private void HandleRequestHossEvent(HossRequestedEvent e)
    {
        this.stage = RoundStage.Hossinng;
        var hossingPlayer = e.HossRequest.PlayerId;
        var hossPartner = this.GetHossingPartner(hossingPlayer);
        this.DealForPlayer(hossingPlayer).GiveCard(e.HossRequest.Card);

        this.MakeCurrentPlayer(hossPartner);
    }

    private void HandlePartnerHossCardGiven(PartnerHossCardGivenEvent e)
    {
        var hossPartner = e.HossPartnerCard.PlayerId;
        var hossingPlayer = this.GetHossingPartner(hossPartner);
        this.stage = RoundStage.Bidding;
        this.DealForPlayer(e.HossPartnerCard.PlayerId).GiveCard(e.HossPartnerCard.Card);
        this.DealForPlayer(hossingPlayer).ReceiveCard(e.HossPartnerCard.Card);
        this.RemovePlayerFromRound(hossPartner);
        this.MakeCurrentPlayer(hossingPlayer);
    }

    private void RemovePlayerFromRound(PlayerId hossPartner)
    {
        this.MakeCurrentPlayer(hossPartner);
        this.teamPlayers.Dequeue();
    }

    private PlayerId GetHossingPartner(PlayerId playerId)
    {
        var hossingTeam = this.GetHossingTeam(playerId);
        var hossPartner = this.teamPlayers.First(t => t.TeamId == hossingTeam && t.PlayerId != playerId)
            .PlayerId;
        return hossPartner;
    }

    private TeamId GetHossingTeam(PlayerId hossRequestPlayerId)
    {
        return this.teamPlayers.FirstOrDefault(t => t.PlayerId == hossRequestPlayerId)?.TeamId ??
               this.teamPlayers.GroupBy(t => t.TeamId).OrderBy(g => g.Count()).First().First().TeamId;
    }

    private void HandleRoundPlayedEvent(RoundPlayedEvent roundPlayedEvent)
    {
        this.stage = RoundStage.Played;
        this.Score = this.Score + roundPlayedEvent.RoundScore;
    }

    private static List<ADeal> DealCards(Deck deck, IEnumerable<RoundPlayer> teamPlayers)
    {
        var playerHand = teamPlayers.Select(p => new ADeal(p.PlayerId)).ToList();

        while (deck.HasCards) playerHand.ForEach(p => p.ReceiveCard(deck.Deal()));

        return playerHand;
    }

    private Bid WinningBid()
    {
        return this.bids.OrderByDescending(b => b.Value).First();
    }

    private void OrderPlayers(IEnumerable<RoundPlayer> players, int roundNumber)
    {
        var teamPlayerList = players.OrderBy(t => t.TeamId).ToList();
        var secondPlayer = teamPlayerList.First(t => t.TeamId == TeamId.Team2);
        var thirdPlayer = teamPlayerList.Last(t => t.TeamId == TeamId.Team1);
        teamPlayerList[2] = thirdPlayer;
        teamPlayerList[1] = secondPlayer;

        var roundPlayers = new Queue<RoundPlayer>(teamPlayerList);
        RotateFirstPlayer(roundNumber, roundPlayers);

        this.teamPlayers = roundPlayers;
    }

    private static void RotateFirstPlayer(int roundNumber, Queue<RoundPlayer> roundPlayers)
    {
        for (var i = 0; i < 4 - roundNumber % 4; i++) roundPlayers.Enqueue(roundPlayers.Dequeue());
    }

    private void HandleStartedEvent(RoundStartedEvent e)
    {
        this.stage = RoundStage.ShufflingCards;
        this.teamPlayers = new Queue<RoundPlayer>(e.TeamPlayers.ToList());
    }

    private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
    {
        this.stage = RoundStage.DealingCards;
        this.deals.Add(e.Deal);
    }

    private void HandleCardsDealtEvent()
    {
        this.stage = RoundStage.Bidding;
    }

    private void HandleBidEvent(BidEvent e)
    {
        this.Play(() => this.bids.Add(e.Bid));
    }

    private void HandleBidCompleteEvent(BidCompleteEvent e)
    {
        this.MakeCurrentPlayer(e.WinningBid.PlayerId);

        this.stage = RoundStage.SelectingTrump;
    }

    private void MakeCurrentPlayer(PlayerId playerId)
    {
        while (this.CurrentPlayerId != playerId) this.FinishTurn();
    }

    private void HandleTrumpSelectedEvent(TrumpSelectedEvent e)
    {
        this.trumpSelection = new TrumpSelection(e.PlayerId, e.Trump);
        this.stage = RoundStage.PlayingCards;
    }

    private void HandleCardPlayedEvent(CardPlayedEvent @event)
    {
        this.Play
        (() =>
        {
            CardPlay cardPlay = new(@event.PlayerId, @event.Card);
            this.tableCenter = this.tableCenter.Push(cardPlay);
            this.deals.First(d => d.PlayerId == @event.PlayerId).PlayCard(@event.Card);
        });
    }

    private void HandleHandPlayedEvent(TrickPlayedEvent @event)
    {
        this.tricks.Push(Trick.FromTableCenter(this.tableCenter, this.trumpSelection));
        var trickWinner = this.tricks.First().Winner;
        while (this.CurrentPlayerId !=
               trickWinner)
            this.FinishTurn();
        this.tableCenter = TableCenter.Clear();
    }

    private void Play(Action action)
    {
        action();
        this.FinishTurn();
    }

    private void FinishTurn()
    {
        this.teamPlayers.Enqueue(this.teamPlayers.Dequeue());
    }
}