// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

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

   public abstract void JoinPlayerToTeam(PlayerId playerId, TeamId teamId);

   public abstract void CreateNewGame(PlayerId playerId);

   public abstract void TeamPlayerReady(PlayerId playerId);
}

public class NoGame : Game
{
   public NoGame()
      : base(new NoGameId())
   {
   }

   public override bool IsNull => true;

   public override void JoinPlayerToTeam(PlayerId playerId, TeamId teamId)
   {
   }

   public override void CreateNewGame(PlayerId playerId)
   {
   }

   public override void TeamPlayerReady(PlayerId playerId)
   {
   }
}
