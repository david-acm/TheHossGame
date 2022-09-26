// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public abstract class Game : EntityBase<AGameId>, IAggregateRoot
{
   protected Game(AGameId id)
      : base(id)
   {
   }

   public enum TeamId
   {
      Team1,
      Team2,
   }

   public abstract void JoinPlayer(APlayerId playerId, TeamId teamId);
}

public class AGame : Game
{
   private readonly List<TeamPlayer> teamPlayers = new ();

   private AGame(APlayerId playerId)
      : base(new GameId())
   {
      this.PlayerId = playerId;
   }

   public APlayerId PlayerId { get; }

   public static AGame StartForPlayer(APlayerId playerId)
   {
      AGame game = new (playerId);
      game.RaiseDomainEvent(new GameStartedEvent(playerId));
      game.RaiseDomainEvent(new PlayerJoinedEvent(playerId, TeamId.Team1));

      return game;
   }

   public IReadOnlyCollection<TeamPlayer> TeamPlayers(TeamId teamId) => this.teamPlayers.Where(p => p.TeamId == teamId)
      .ToList().AsReadOnly();

   public override void JoinPlayer(APlayerId playerId, TeamId teamId)
      => this.Apply(new PlayerJoinedEvent(playerId, teamId));

   protected override void When(DomainEventBase @event)
   {
      switch (@event)
      {
         case PlayerJoinedEvent e:
            this.teamPlayers.Add(new TeamPlayer(e.PlayerId, e.TeamId));
            break;

         default:
            break;
      }
   }

   protected override void EnsureValidState()
   {
      var valid =
         ValidTeamPlayers(TeamId.Team1) &&
         ValidTeamPlayers(TeamId.Team2);

      if (!valid)
      {
         throw new InvalidEntityStateException();
      }

      bool ValidTeamPlayers(TeamId team1) =>
         this.TeamPlayers(team1).Count <= 2;
   }
}

public class NoGame : Game
{
   public NoGame()
      : base(new NoGameId())
   {
   }

   public override void JoinPlayer(APlayerId playerId, TeamId teamId)
   {
   }

   protected override void EnsureValidState()
   {
   }

   protected override void When(DomainEventBase @event)
   {
   }
}