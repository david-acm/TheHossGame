// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class Game : EntityBase<GameId>, IAggregateRoot
{
   private readonly List<TeamPlayer> teamPlayers = new ();

   private Game(PlayerId playerId)
      : base(new GameId())
   {
      this.PlayerId = playerId;
   }

   public enum TeamId
   {
      Team1,
      Team2,
   }

   public PlayerId PlayerId { get; }

   public static Game StartForPlayer(PlayerId playerId)
   {
      Game game = new (playerId);
      game.RaiseDomainEvent(new GameStartedEvent(playerId));
      game.RaiseDomainEvent(new PlayerJoinedEvent(playerId, TeamId.Team1));

      return game;
   }

   public IReadOnlyCollection<TeamPlayer> TeamPlayers(TeamId teamId) => this.teamPlayers.Where(p => p.TeamId == teamId)
      .ToList().AsReadOnly();

   public void JoinPlayer(PlayerId playerId, TeamId teamId)
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
