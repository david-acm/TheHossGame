// 🃏 The HossGame 🃏
// <copyright file="AGame.Behaviour.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using System.Linq;
using TheHossGame.Core.GameAggregate.Events;
using TheHossGame.Core.GameAggregate.PlayerEntity;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static Game.TeamId;

/// <summary>
/// Behaviour side.
/// </summary>
public sealed partial class AGame
{
   private readonly IShufflingService shufflingService;

   protected override bool IsNull => false;

   public static AGame CreateForPlayer(
      PlayerId playerId,
      IShufflingService shufflingService)
   {
      AGame game = new (shufflingService);
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

      this.Apply(new GameStartedEvent(
         this.Id,
         round.Id,
         round.TeamPlayers,
         round.PlayerDeals,
         round.Bids));
   }

   public void Bid(PlayerId playerId, BidValue value) => this.NewestRound.Bid(playerId, value);

   public void SelectTrump(PlayerId currentPlayerId, CardSuit suit) => this.NewestRound.SelectTrump(currentPlayerId, suit);

   public void Finish() => this.Apply(new GameFinishedEvent(this.Id));

   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         GameState.Created => this.TeamValid(Team1) && this.TeamValid(Team2),
         GameState.TeamsFormed => this.TeamComplete(Team1) && this.TeamComplete(Team2),
         GameState.Started => this.FindTeamPlayers().All(t => t.IsReady),
         GameState.Finished => throw new NotImplementedException(),
         _ => throw new NotImplementedException(),
      };

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }
   }

   protected override void When(DomainEventBase @event) => (@event switch
   {
      PlayerJoinedEvent e => (Action)(() => HandleJoin(e)),
      NewGameCreatedEvent => HandleGameCreated,
      TeamsFormedEvent => HandleTeamsFormedEvent,
      GameStartedEvent e => () => HandleGameStartedEvent(e),
      GameFinishedEvent => () => this.State = GameState.Finished,
      _ => () => { },
   }).Invoke();

   private static void HandleGameCreated()
   {
   }

   private void CreateNewGame(PlayerId playerId)
      => this.Apply(new NewGameCreatedEvent(this.Id, playerId));

   private bool TeamsAreComplete() => this.TeamComplete(Team1) && this.TeamComplete(Team2);

   private bool PlayersNotReady()
      => !this.FindTeamPlayers().All(p => p.IsReady) ||
         !this.TeamComplete(Team1) ||
         !this.TeamComplete(Team2);

   private void HandleGameStartedEvent(GameStartedEvent @event)
   {
      var round = ARound.FromStream(@event, this.Apply);
      this.rounds.Add(round);

      this.State = GameState.Started;
   }

   private IEnumerable<RoundPlayer> GetTeamPlayers() => this.FindTeamPlayers().Select(g => new RoundPlayer(g.Id, g.TeamId));

   private void HandleTeamsFormedEvent() => this.State = GameState.TeamsFormed;

   private void HandleJoin(PlayerJoinedEvent e)
   {
      var teamPlayer = AGamePlayer.FromStream(e, this.Apply);
      this.gamePlayers.Add(teamPlayer);
   }

   private bool TeamValid(TeamId team1) => this.FindGamePlayers(team1).Count <= 2;

   private bool TeamComplete(TeamId team1) => this.FindGamePlayers(team1).Count == 2;
}