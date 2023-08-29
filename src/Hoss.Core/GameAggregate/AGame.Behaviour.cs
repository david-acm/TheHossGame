// 🃏 The HossGame 🃏
// <copyright file="AGame.Behaviour.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

#region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.PlayerEntity;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.Interfaces;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;
using static Game.TeamId;

#endregion

/// <summary>
///    Behaviour side.
/// </summary>
public sealed partial class AGame
{
    private readonly IShufflingService shufflingService;

    public static AGame CreateForPlayer(PlayerId playerId, IShufflingService shufflingService)
    {
        AGame game = new(shufflingService);
        game.CreateNewGame(playerId);
        game.JoinPlayerToTeam(playerId, Team1);

        return game;
    }

    public void JoinPlayerToTeam(PlayerId playerId, TeamId teamId)
    {
        this.FindPlayer(playerId).Join(teamId);

        if (this.TeamsAreComplete())
        {
            this.Apply(new TeamsFormedEvent(this.Id));
        }
    }

    public void TeamPlayerReady(PlayerId playerId)
    {
        this.FindPlayer(playerId).Ready();

        if (this.PlayersNotReady())
        {
            return;
        }

        var shuffledDeck = ADeck.ShuffleNew(this.shufflingService);
        var round = ARound.StartNew(this.Id, this.GetTeamPlayers(), shuffledDeck, this.Apply);

        this.Apply(new GameStartedEvent(this.Id, round.Id, round.RoundPlayers, round.Deals, round.Bids));
    }

    public void Bid(PlayerId playerId, BidValue value)
    {
        this.CurrentRound.Bid(playerId, value);
    }

    public void SelectTrump(PlayerId currentPlayerId, Suit suit)
    {
        this.CurrentRound.SelectTrump(currentPlayerId, suit);
    }

    public void PlayCard(PlayerId playerId, Card card)
    {
        this.CurrentRound.PlayCard(playerId, card);
    }

    public void Finish()
    {
        this.Apply(new GameFinishedEvent(this.Id));
    }

    protected override void EnsureValidState()
    {
#pragma warning disable CS8524
        var valid = this.State switch
#pragma warning restore CS8524
        {
            GameState.Created => this.TeamValid(Team1) && this.TeamValid(Team2),
            GameState.TeamsFormed => this.TeamComplete(Team1) && this.TeamComplete(Team2),
            GameState.Started => this.FindGamePlayers().All(t => t.IsReady),
            GameState.Finished => this.State == GameState.Finished,
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
            PlayerJoinedEvent e => (Action) (() => this.HandleJoin(e)),
            NewGameCreatedEvent => HandleGameCreated,
            TeamsFormedEvent => this.HandleTeamsFormedEvent,
            GameStartedEvent e => () => this.HandleGameStartedEvent(e),
            GameFinishedEvent => () => this.State = GameState.Finished,
            RoundPlayedEvent => () => this.HandleRoundPlayedEvent(),
            _ => () => { },
        }).Invoke();
    }

    private static void HandleGameCreated()
    {
    }

    private void CreateNewGame(PlayerId playerId)
    {
        this.Apply(new NewGameCreatedEvent(this.Id, playerId));
    }

    private bool TeamsAreComplete()
    {
        return this.TeamComplete(Team1) && this.TeamComplete(Team2);
    }

    private bool PlayersNotReady()
    {
        return !this.FindGamePlayers().All(p => p.IsReady) || !this.TeamComplete(Team1) || !this.TeamComplete(Team2);
    }

    private void HandleGameStartedEvent(GameStartedEvent @event)
    {
        var round = ARound.FromStream(@event, this.Apply);
        this.rounds.Add(round);

        this.State = GameState.Started;
    }

    private void HandleRoundPlayedEvent()
    {
    }

    private IEnumerable<RoundPlayer> GetTeamPlayers()
    {
        return this.FindGamePlayers().Select(g => new RoundPlayer(g.Id, g.TeamId));
    }

    private void HandleTeamsFormedEvent()
    {
        this.State = GameState.TeamsFormed;
    }

    private void HandleJoin(PlayerJoinedEvent e)
    {
        var teamPlayer = AGamePlayer.FromStream(e, this.Apply);
        this.gamePlayers.Add(teamPlayer);
    }

    private bool TeamValid(TeamId team1)
    {
        return this.FindGamePlayers(team1).Count <= 2;
    }

    private bool TeamComplete(TeamId team1)
    {
        return this.FindGamePlayers(team1).Count == 2;
    }
}