// 🃏 The HossGame 🃏
// <copyright file="AGame.Behaviour.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;
using static Hoss.Core.GameAggregate.GameEvents;
using static Hoss.Core.GameAggregate.TeamId;

namespace Hoss.Core.GameAggregate;

#region

using PlayerEntity;
using RoundEntity;
using RoundEntity.DeckValueObjects;
using Interfaces;
using static RoundEntity.RoundEvents;

#endregion

/// <summary>
///    Behaviour side.
/// </summary>
public sealed partial class AGame
{
  private readonly IShufflingService shufflingService;

  public static AGame CreateForPlayer(AGameId gameId, PlayerId playerId, IShufflingService shufflingService)
  {
    AGame game = new(gameId, shufflingService);
    game.CreateNewGame(playerId);
    game.JoinPlayerToTeam(playerId, NorthSouth);

    return game;
  }

  public void JoinPlayerToTeam(PlayerId playerId, TeamId teamId)
  {
    FindPlayer(playerId).Join(teamId);

    if (TeamsAreComplete())
    {
      Apply(new TeamsFormedEvent(Id));
    }
  }

  public void TeamPlayerReady(PlayerId playerId)
  {
    FindPlayer(playerId).Ready();

    if (PlayersNotReady())
    {
      return;
    }

    var shuffledDeck = ADeck.ShuffleNew(shufflingService);
    var round = Round.StartNew(Id, GetTeamPlayers(), shuffledDeck, Apply);

    Apply(new GameStartedEvent(Id, round.Id, round.RoundPlayers, round.Deals, round.Bids));
  }

  public void Bid(PlayerId playerId, BidValue value)
  {
    // TO DO: Hoss without help
    CurrentRoundBase.Bid(playerId, value);
  }

  public void RequestHoss(PlayerId playerId, Card card)
  {
    CurrentRoundBase.RequestHoss(playerId, card);
  }

  public void GiveHossCard(PlayerId playerId, Card card)
  {
    CurrentRoundBase.GiveHossCard(playerId, card);
  }

  public void SelectTrump(PlayerId currentPlayerId, Suit suit)
  {
    CurrentRoundBase.SelectTrump(currentPlayerId, suit);
  }

  public void PlayCard(PlayerId playerId, Card card)
  {
    CurrentRoundBase.PlayCard(playerId, card);
  }

  public void Finish()
  {
    Apply(new GameFinishedEvent(Id));
  }

  protected override void EnsureValidState()
  {
#pragma warning disable CS8524
    var valid = Stage switch
#pragma warning restore CS8524
    {
      GameState.Created => TeamValid(NorthSouth) && TeamValid(EastWest),
      GameState.TeamsFormed => TeamComplete(NorthSouth) && TeamComplete(EastWest),
      GameState.Started => FindGamePlayers().All(t => t.IsReady),
      GameState.Finished => Stage == GameState.Finished,
    };

    if (!valid)
    {
      throw new InvalidEntityStateException();
    }
  }

  protected override void When(DomainEventBase @event)
  {
    (@event switch
    {
      PlayerJoinedEvent e => (Action)(() => HandleJoin(e)),
      NewGameCreatedEvent => HandleGameCreated,
      TeamsFormedEvent => HandleTeamsFormedEvent,
      PlayerReadyEvent e => () => HandlePlayerReadyEvent(e),
      RoundStartedEvent e => () => HandleRoundStartedEvent(e),
      PlayerCardsDealtEvent e => () => this.CurrentRoundBase.Load(new [] { e }),
      AllCardsDealtEvent e => () => this.CurrentRoundBase.Load(new [] { e }), 
      BidEvent e => () => this.CurrentRoundBase.Load(new[] { e }),
      BidCompleteEvent e => () => HandleBidCompleteEvent(e),
      GameStartedEvent e => () => HandleGameStartedEvent(e),
      GameFinishedEvent => () => HandleGameFinishedEvent(),
      RoundPlayedEvent e => () => HandleRoundPlayedEvent(e),
      _ => () => { },
    }).Invoke();
  }

  private void HandleRoundStartedEvent(RoundStartedEvent @event)
  {
    var round = Round.FromStream(@event, Apply);
    rounds.Add(round);
  }

  private void HandleBidCompleteEvent(BidCompleteEvent e)
  {
    this.CurrentRoundBase.Load(new[] { e });
  }

  private GameState HandleGameFinishedEvent()
  {
    return Stage = GameState.Finished;
  }

  private static void HandleGameCreated()
  {
  }

  private void CreateNewGame(PlayerId playerId)
  {
    Apply(new NewGameCreatedEvent(Id, playerId));
  }

  private bool TeamsAreComplete()
  {
    return TeamComplete(NorthSouth) && TeamComplete(EastWest);
  }

  private bool PlayersNotReady()
  {
    return !FindGamePlayers().All(p => p.IsReady) || !TeamComplete(NorthSouth) || !TeamComplete(EastWest);
  }

  private void HandlePlayerReadyEvent(PlayerReadyEvent @event)
  {
    FindPlayer(@event.PlayerId).Load(new[] { @event });
  }

  private void HandleGameStartedEvent(GameStartedEvent @event)
  {
    Stage = GameState.Started;
  }

  private void HandleRoundPlayedEvent(RoundPlayedEvent e)
  {
    Score = Score.AddRoundScore(e.RoundScore);
    if (Score >= MaxScore)
    {
      Apply(new GameFinishedEvent(Id));
      return;
    }

    rounds.Add(Round.StartNew(Id, new Queue<RoundPlayer>(GetTeamPlayers()),
      ADeck.ShuffleNew(shufflingService), Apply, rounds.Count));
  }

  private IEnumerable<RoundPlayer> GetTeamPlayers()
  {
    return FindGamePlayers().Select(g => new RoundPlayer(g.Id, g.TeamId));
  }

  private void HandleTeamsFormedEvent()
  {
    Stage = GameState.TeamsFormed;
  }

  private void HandleJoin(PlayerJoinedEvent e)
  {
    var teamPlayer = AGamePlayer.FromStream(e, Apply);
    gamePlayers.Add(teamPlayer);
  }

  private bool TeamValid(TeamId team1)
  {
    return FindGamePlayers(team1).Count <= 2;
  }

  private bool TeamComplete(TeamId team1)
  {
    return FindGamePlayers(team1).Count == 2;
  }
}