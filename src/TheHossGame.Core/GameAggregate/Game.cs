// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using Ardalis.Specification;
using System.Linq;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class AGame : Game
{
   private readonly List<TeamPlayer> teamPlayers = new ();

   private AGame(PlayerId playerId)
      : base(new AGameId())
   {
      this.PlayerId = playerId;
   }

   public enum GameState
   {
      Created,
      TeamsFormed,
   }

   public PlayerId PlayerId { get; }

   public GameState State { get; private set; }

   public static AGame CreateNewForPlayer(PlayerId playerId)
   {
      AGame game = new (playerId);
      game.CreateNewGame(playerId);
      game.JoinPlayer(playerId, TeamId.Team1);

      return game;
   }

   public IReadOnlyCollection<TeamPlayer> FindTeamPlayers(TeamId teamId)
      => this.teamPlayers.Where(p => p.TeamId == teamId)
         .ToList().AsReadOnly();

   public IReadOnlyCollection<TeamPlayer> FindTeamPlayers()
      => this.teamPlayers.ToList().AsReadOnly();

   public TeamPlayer FindSinglePlayer(PlayerId playerId)
      => this.FindTeamPlayers().FirstOrDefault(p => p.PlayerId == playerId)
         ?? new NoTeamPlayer();

   public override void CreateNewGame(PlayerId playerId)
      => this.Apply(new NewGameCreatedEvent(this.Id, playerId));

   public override void JoinPlayer(PlayerId playerId, TeamId teamId)
   {
      bool playerIsMember = this.FindSinglePlayer(playerId) is not NoTeamPlayer;
      if (playerIsMember)
      {
         this.Apply(new PlayerAlreadyInGame(playerId));
         return;
      }

      this.Apply(new PlayerJoinedEvent(playerId, teamId));

      if (this.TeamComplete(TeamId.Team1) && this.TeamComplete(TeamId.Team2))
      {
         this.Apply(new TeamsFormedEvent(this.Id));
      }
   }

   public void TeamPlayerReady(PlayerId playerId)
   {
      bool playerIsNotMember = this.FindSinglePlayer(playerId) is NoTeamPlayer;
      if (playerIsNotMember)
      {
         return;
      }

      this.Apply(new PlayerReadyEvent(this.Id, playerId));
   }

   protected static void ApplyToEntity(IInternalEventHandler eventHandler, PlayerJoinedEvent @event) => eventHandler.Handle(@event);

   protected override void EnsureValidState()
   {
      var valid = this.State switch
      {
         GameState.Created => this.TeamValid(TeamId.Team1) && this.TeamValid(TeamId.Team2),
         GameState.TeamsFormed => this.TeamComplete(TeamId.Team1) && this.TeamComplete(TeamId.Team2),
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
      _ => () => { },
   }).Invoke();

   private void HandlePlayerReadyEvent(PlayerReadyEvent e)
   {
      var player = this.FindSinglePlayer(e.PlayerId);
      this.ApplyToEntity(player, e);
   }

   private void HandleTeamsFormedEvent()
   {
      this.State = GameState.TeamsFormed;
   }

   private void HandleNameCreated(NewGameCreatedEvent e)
   {
      this.Id = e.GameId;
   }

   private void HandleJoin(PlayerJoinedEvent e)
   {
      ATeamPlayer teamPlayer = new(e.PlayerId, e.TeamId, this.Apply);
      ApplyToEntity(teamPlayer, e);
      this.teamPlayers.Add(teamPlayer);
   }

   private bool TeamValid(TeamId team1) =>
            this.FindTeamPlayers(team1).Count <= 2;

   private bool TeamComplete(TeamId team1) =>
            this.FindTeamPlayers(team1).Count == 2;
}

public abstract class Game : AggregateRoot<GameId>, IAggregateRoot
{
   protected Game(GameId id)
      : base(id)
   {
   }

   public enum TeamId
   {
      NoTeamId,
      Team1,
      Team2,
   }

   public abstract void JoinPlayer(PlayerId playerId, TeamId teamId);

   public abstract void CreateNewGame(PlayerId playerId);
}

public class NoGame : Game
{
   public NoGame()
      : base(new NoGameId())
   {
   }

   public override void JoinPlayer(PlayerId playerId, TeamId teamId)
   {
   }

   public override void CreateNewGame(PlayerId playerId)
   {
   }
}

public static class ListExtensions
{
   public static bool Replace<T>(this List<T> list, Func<T, bool> selector, Func<T, T> modifier)
   {
      var item = list.FirstOrDefault(selector);
      if (item is null)
      {
         return false;
      }

      list.Remove(item);
      list.Add(modifier(item));
      return true;
   }
}
