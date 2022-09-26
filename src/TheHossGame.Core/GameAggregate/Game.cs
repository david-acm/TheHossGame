// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

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

   public IReadOnlyCollection<TeamPlayer> TeamPlayers(TeamId teamId)
      => this.teamPlayers.Where(p => p.TeamId == teamId)
         .ToList().AsReadOnly();

   public override void CreateNewGame(PlayerId playerId)
      => this.Apply(new NewGameCreatedEvent(playerId));

   public override void JoinPlayer(PlayerId playerId, TeamId teamId)
   {
      if (this.teamPlayers.Any(t => t.PlayerId == playerId))
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

   protected override void When(DomainEventBase @event)
   {
      switch (@event)
      {
         case PlayerJoinedEvent e:
            this.teamPlayers.Add(new TeamPlayer(e.PlayerId, e.TeamId));
            break;

         case NewGameCreatedEvent e:
            break;

         case TeamsFormedEvent e:
            this.State = GameState.TeamsFormed;
            break;

         default:
            break;
      }
   }

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

   private bool TeamValid(TeamId team1) =>
            this.TeamPlayers(team1).Count <= 2;

   private bool TeamComplete(TeamId team1) =>
            this.TeamPlayers(team1).Count == 2;
}

public abstract class Game : EntityBase<GameId>, IAggregateRoot
{
   protected Game(GameId id)
      : base(id)
   {
   }

   public enum TeamId
   {
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

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}