// 🃏 The HossGame 🃏
// <copyright file="AGame.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using Ardalis.Specification;
using System.Linq;

/// <summary>
/// State side.
/// </summary>
public partial class AGame : Game
{
   private readonly List<TeamPlayer> teamPlayers = new ();
   private readonly List<RoundId> roundIds = new ();

   private AGame()
      : base(new AGameId())
   {
   }

   public enum GameState
   {
      Created,
      TeamsFormed,
      Started,
   }

   public GameState State { get; private set; }

   public IReadOnlyList<RoundId> RoundIds => this.roundIds.AsReadOnly();

   public IReadOnlyCollection<TeamPlayer> FindTeamPlayers(TeamId teamId)
      => this.teamPlayers.Where(p => p.TeamId == teamId)
         .ToList().AsReadOnly();

   public IReadOnlyCollection<TeamPlayer> FindTeamPlayers()
      => this.teamPlayers.ToList().AsReadOnly();
}
