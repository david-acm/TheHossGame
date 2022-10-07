﻿// 🃏 The HossGame 🃏
// <copyright file="AGame.Behaviour.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using System.Linq;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.SharedKernel;
using static Game.TeamId;

/// <summary>
/// Behaviour side.
/// </summary>
public partial class AGame : Game
{
   private readonly IShufflingService shufflingService;

   public static AGame CreateForPlayer(
      PlayerId playerId,
      IShufflingService shufflingService)
   {
      AGame game = new (shufflingService);
      game.CreateNewGame(playerId);
      game.JoinPlayerToTeam(playerId, Team1);

      return game;
   }

   public GamePlayer FindSinglePlayer(PlayerId playerId)
      => this.FindTeamPlayers().FirstOrDefault(p => p.PlayerId == playerId)
         ?? new NoGamePlayer();

   public override void CreateNewGame(PlayerId playerId)
      => this.Apply(new NewGameCreatedEvent(this.Id, playerId));

   public override void JoinPlayerToTeam(PlayerId playerId, TeamId teamId)
   {
      bool playerIsMember = this.FindSinglePlayer(playerId) is not NoGamePlayer;
      if (playerIsMember)
      {
         this.Apply(new PlayerAlreadyInGame(playerId));
         return;
      }

      this.Apply(new PlayerJoinedEvent(playerId, teamId));

      if (this.TeamComplete(Team1) && this.TeamComplete(Team2))
      {
         this.Apply(new TeamsFormedEvent(this.Id));
      }
   }

   public override void TeamPlayerReady(PlayerId playerId)
   {
      bool playerIsNotMember = this.FindSinglePlayer(playerId) is NoGamePlayer;
      if (playerIsNotMember)
      {
         return;
      }

      this.Apply(new PlayerReadyEvent(this.Id, playerId));

      if (this.AreAllPlayersReady())
      {
         this.Apply(new GameStartedEvent(this.Id));
         this.StartNewRound();
      }
   }

   public void Bid(BidCommand bidCommand) => this.LastRound.Bid(bidCommand);

   protected static void ApplyToEntity(IInternalEventHandler eventHandler, PlayerJoinedEvent @event) => eventHandler.Handle(@event);

   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         GameState.Created => this.TeamValid(Team1) && this.TeamValid(Team2),
         GameState.TeamsFormed => this.TeamComplete(Team1) && this.TeamComplete(Team2),
         GameState.Started => this.FindTeamPlayers().All(t => t.IsReady),
         _ => throw new NotImplementedException()
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   protected override void When(DomainEventBase @event) => (@event switch
   {
      PlayerJoinedEvent e => (Action)(() => HandleJoin(e)),
      NewGameCreatedEvent e => () => HandleNameCreated(e),
      TeamsFormedEvent e => () => HandleTeamsFormedEvent(),
      PlayerReadyEvent e => () => HandlePlayerReadyEvent(e),
      GameStartedEvent e => () => HandleGameStartedEvent(),
      RoundStartedEvent e => () => HandleRoundStartedEvent(e),
      _ => () => { },
   }).Invoke();

   private void StartNewRound()
   {
      ARound.StartNew(
         this.Id,
         this.FindTeamPlayers().Select(g => new TeamPlayer(g.Id, g.TeamId)),
         this.shufflingService,
         this.Apply);
   }

   private bool AreAllPlayersReady() => this.FindTeamPlayers().All(p => p.IsReady) && this.TeamComplete(Team1) && this.TeamComplete(Team2);

   private void HandleRoundStartedEvent(RoundStartedEvent e) => this.rounds.Add(e.Round);

   private void HandleGameStartedEvent() => this.State = GameState.Started;

   private void HandlePlayerReadyEvent(PlayerReadyEvent e)
   {
      var player = this.FindSinglePlayer(e.PlayerId);
      this.ApplyToEntity(player, e);
   }

   private void HandleTeamsFormedEvent() => this.State = GameState.TeamsFormed;

   private void HandleNameCreated(NewGameCreatedEvent e) => this.Id = e.GameId;

   private void HandleJoin(PlayerJoinedEvent e)
   {
      AGamePlayer teamPlayer = new (e.PlayerId, e.TeamId, this.Apply);
      ApplyToEntity(teamPlayer, e);
      this.teamPlayers.Add(teamPlayer);
   }

   private bool TeamValid(TeamId team1) => this.FindTeamPlayers(team1).Count <= 2;

   private bool TeamComplete(TeamId team1) => this.FindTeamPlayers(team1).Count == 2;
}
