// 🃏 The HossGame 🃏
// <copyright file="ARound.Behavior.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

using DeckValueObjects;
using RoundScoreValueObject;
using static RoundEvents;

#endregion

/// <summary>
///     The behaviour side.
/// </summary>
public sealed partial class Round
{
  private PlayerId BidWinner => WinningBid().PlayerId;

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
  
  internal static Round FromStream(RoundEvents.RoundStartedEvent @event, Action<DomainEventBase> when)
  {
    return new Round(@event.GameId, @event.RoundId, when)
    {
      teamPlayers = new Queue<RoundPlayer>(@event.TeamPlayers),
      stage = RoundStage.Bidding,
    };
  }

  internal override void Bid(PlayerId playerId, BidValue value)
  {
    Apply(new BidEvent(GameId, Id, new Bid(playerId, value)));

    if (bids.Count == 4 && Stage != RoundStage.Hossinng)
      Apply(new BidCompleteEvent(GameId, Id, WinningBid()));
  }

  /// <inheritdoc />
  internal override void RequestHoss(PlayerId playerId, Card card)
  {
    Apply(new HossRequestedEvent(GameId, Id, new HossRequest(playerId, card)));
  }

  /// <inheritdoc />
  internal override void GiveHossCard(PlayerId playerId, Card card)
  {
    Apply(new PartnerHossCardGivenEvent(GameId, Id, new HossPartnerCard(playerId, card)));
    Apply(new BidEvent(GameId, Id, new Bid(GetHossingPartner(playerId), BidValue.Hoss)));
    Apply(new BidCompleteEvent(GameId, Id, WinningBid()));
  }

  internal override void SelectTrump(PlayerId playerId, Suit suit)
  {
    Apply(new TrumpSelectedEvent(GameId, Id, playerId, suit));
  }

  internal override void PlayCard(PlayerId playerId, Card card)
  {
    Apply(new CardPlayedEvent(GameId, Id, playerId, card));
    if (CardsPlayed.Count == RoundPlayers.Count)
      Apply(new TrickPlayedEvent(GameId, Id, GetHandWinner()));

    if (tricks.Count != 6) return;
    var bidWinningTeam = GetBiddingTeam();
    Apply(new RoundPlayedEvent(
      GameId,
      Id, RoundScore.FromRound(bidWinningTeam, tricks, WinningBid())));
  }

  private (RoundPlayer, RoundPlayer) GetBiddingTeam()
  {
    var bidWinningPlayerId = WinningBid().PlayerId;
    var biddingTeam = teamPlayers.FirstOrDefault(t => t.PlayerId == bidWinningPlayerId)?.TeamId ??
                      GetHossingTeam(bidWinningPlayerId);
    var bidWinningTeam = (teamPlayers.First(t => t.TeamId == biddingTeam),
      teamPlayers.Last(t => t.TeamId == biddingTeam));
    return bidWinningTeam;
  }

  private CardPlay GetHandWinner()
  {
    return CardsPlayed.OrderByDescending(c => c.Card,
        new Card.CardComparer(SelectedTrump, CardsPlayed.Last().Card.Suit))
      .First();
  }

  protected override void When(DomainEventBase @event)
  {
    var roundEvent = (RoundEventBase)@event;
#pragma warning disable CS8509
    (roundEvent switch
#pragma warning restore CS8509
    {
      RoundStartedEvent e => (Action)(() => HandleStartedEvent(e)),
      PlayerCardsDealtEvent e => () => HandlePlayerCardsDealtEvent(e),
      AllCardsDealtEvent => HandleCardsDealtEvent,
      BidEvent e => () => HandleBidEvent(e),
      HossRequestedEvent e => () => HandleRequestHossEvent(e),
      PartnerHossCardGivenEvent e => () => HandlePartnerHossCardGiven(e),
      BidCompleteEvent e => () => HandleBidCompleteEvent(e),
      TrumpSelectedEvent e => () => HandleTrumpSelectedEvent(e),
      CardPlayedEvent e => () => HandleCardPlayedEvent(e),
      TrickPlayedEvent e => () => HandleHandPlayedEvent(e),
      RoundPlayedEvent e => () => HandleRoundPlayedEvent(e),
    }).Invoke();
  }

  private void HandleRequestHossEvent(HossRequestedEvent e)
  {
    stage = RoundStage.Hossinng;
    var hossingPlayer = e.HossRequest.PlayerId;
    var hossPartner = GetHossingPartner(hossingPlayer);
    DealForPlayer(hossingPlayer).GiveCard(e.HossRequest.Card);

    MakeCurrentPlayer(hossPartner);
  }

  private void HandlePartnerHossCardGiven(PartnerHossCardGivenEvent e)
  {
    var hossPartner = e.HossPartnerCard.PlayerId;
    var hossingPlayer = GetHossingPartner(hossPartner);
    stage = RoundStage.Bidding;
    DealForPlayer(e.HossPartnerCard.PlayerId).GiveCard(e.HossPartnerCard.Card);
    DealForPlayer(hossingPlayer).ReceiveCard(e.HossPartnerCard.Card);
    RemovePlayerFromRound(hossPartner);
    MakeCurrentPlayer(hossingPlayer);
  }

  private void RemovePlayerFromRound(PlayerId hossPartner)
  {
    MakeCurrentPlayer(hossPartner);
    teamPlayers.Dequeue();
  }

  private PlayerId GetHossingPartner(PlayerId playerId)
  {
    var hossingTeam = GetHossingTeam(playerId);
    var hossPartner = teamPlayers.First(t => t.TeamId == hossingTeam && t.PlayerId != playerId)
      .PlayerId;
    return hossPartner;
  }

  private TeamId GetHossingTeam(PlayerId hossRequestPlayerId)
  {
    return teamPlayers.FirstOrDefault(t => t.PlayerId == hossRequestPlayerId)?.TeamId ??
           teamPlayers.GroupBy(t => t.TeamId).OrderBy(g => g.Count()).First().First().TeamId;
  }

  private void HandleRoundPlayedEvent(RoundPlayedEvent roundPlayedEvent)
  {
    stage = RoundStage.Played;
    Score = Score + roundPlayedEvent.RoundScore;
  }

  private static List<ADeal> DealCards(Deck deck, IEnumerable<RoundPlayer> teamPlayers)
  {
    var playerHand = teamPlayers.Select(p => new ADeal(p.PlayerId)).ToList();

    while (deck.HasCards) playerHand.ForEach(p => p.ReceiveCard(deck.Deal()));

    return playerHand;
  }

  private Bid WinningBid()
  {
    return bids.OrderByDescending(b => b.Value).First();
  }

  private void OrderPlayers(IEnumerable<RoundPlayer> players, int roundNumber)
  {
    var teamPlayerList = players.OrderBy(t => t.TeamId).ToList();
    var secondPlayer = teamPlayerList.First(t => t.TeamId == TeamId.EastWest);
    var thirdPlayer = teamPlayerList.Last(t => t.TeamId == TeamId.NorthSouth);
    teamPlayerList[2] = thirdPlayer;
    teamPlayerList[1] = secondPlayer;

    var roundPlayers = new Queue<RoundPlayer>(teamPlayerList);
    RotateFirstPlayer(roundNumber, roundPlayers);

    teamPlayers = roundPlayers;
  }

  private static void RotateFirstPlayer(int roundNumber, Queue<RoundPlayer> roundPlayers)
  {
    for (var i = 0; i < 4 - roundNumber % 4; i++) roundPlayers.Enqueue(roundPlayers.Dequeue());
  }

  private void HandleStartedEvent(RoundStartedEvent e)
  {
    stage = RoundStage.ShufflingCards;
    teamPlayers = new Queue<RoundPlayer>(e.TeamPlayers.ToList());
  }

  private void HandlePlayerCardsDealtEvent(PlayerCardsDealtEvent e)
  {
    stage = RoundStage.DealingCards;
    deals.Add(e.Deal);
  }

  private void HandleCardsDealtEvent()
  {
    stage = RoundStage.Bidding;
  }

  private void HandleBidEvent(BidEvent e)
  {
    Play(() => bids.Add(e.Bid));
  }

  private void HandleBidCompleteEvent(BidCompleteEvent e)
  {
    MakeCurrentPlayer(e.WinningBid.PlayerId);

    stage = RoundStage.SelectingTrump;
  }

  private void MakeCurrentPlayer(PlayerId playerId)
  {
    while (CurrentPlayerId.Id != playerId.Id) FinishTurn();
  }

  private void HandleTrumpSelectedEvent(TrumpSelectedEvent e)
  {
    trumpSelection = new TrumpSelection(e.PlayerId, e.Trump);
    stage = RoundStage.PlayingCards;
  }

  private void HandleCardPlayedEvent(CardPlayedEvent @event)
  {
    Play
    (() =>
    {
      CardPlay cardPlay = new(@event.PlayerId, @event.Card);
      tableCenter = tableCenter.Push(cardPlay);
      deals.First(d => d.PlayerId.Id == @event.PlayerId.Id).PlayCard(@event.Card);
    });
  }

  private void HandleHandPlayedEvent(TrickPlayedEvent @event)
  {
    tricks.Push(Trick.FromTableCenter(tableCenter, trumpSelection));
    var trickWinner = tricks.First().Winner;
    while (CurrentPlayerId !=
           trickWinner)
      FinishTurn();
    tableCenter = TableCenter.Clear();
  }

  private void Play(Action action)
  {
    action();
    FinishTurn();
  }

  private void FinishTurn()
  {
    teamPlayers.Enqueue(teamPlayers.Dequeue());
  }
}