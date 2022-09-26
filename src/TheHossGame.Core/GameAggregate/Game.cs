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

public sealed class AGame : Game
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

   public IReadOnlyCollection<TeamPlayer> TeamPlayers()
      => this.teamPlayers.ToList().AsReadOnly();

   public TeamPlayer TeamPlayer(PlayerId playerId)
      => this.TeamPlayers().FirstOrDefault(p => p.PlayerId == playerId)
         ?? new NoTeamPlayer();

   public override void CreateNewGame(PlayerId playerId)
      => this.Apply(new NewGameCreatedEvent(playerId));

   public override void JoinPlayer(PlayerId playerId, TeamId teamId)
   {
      bool playerIsMember = this.TeamPlayer(playerId) is not NoTeamPlayer;
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
      bool playerIsNotMember = this.TeamPlayer(playerId) is NoTeamPlayer;
      if (playerIsNotMember)
      {
         return;
      }

      this.Apply(new PlayerReadyEvent(this.Id, playerId));
   }

   protected override void When(DomainEventBase @event)
   {
      switch (@event)
      {
         case PlayerJoinedEvent e:
            this.teamPlayers.Add(new ATeamPlayer(e.PlayerId, e.TeamId));
            break;

         case NewGameCreatedEvent e:
            break;

         case TeamsFormedEvent e:
            this.State = GameState.TeamsFormed;
            break;

         case PlayerReadyEvent e:
            this.teamPlayers.Replace(
               t => t.PlayerId == e.PlayerId,
               t => t with { IsReady = true });
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

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
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