// 🃏 The HossGame 🃏
// <copyright file="AGame.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using Ardalis.Specification;
using System.Collections.Generic;
using System.Linq;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.RoundAggregate;
using TheHossGame.SharedKernel;

/// <summary>
/// State side.
/// </summary>
public partial class AGame : Game
{
   private readonly List<GamePlayer> teamPlayers = new ();
   private readonly List<RoundId> roundIds = new ();
   private List<Round> rounds = new List<ARound>().Cast<Round>().ToList();

   private AGame(Interfaces.IShufflingService shufflingService)
      : base(new AGameId())
   {
      this.shufflingService = shufflingService;
   }

   public enum GameState
   {
      Created,
      TeamsFormed,
      Started,
   }

   public CurrentRound CurrentRound => new (this.LastRound);

   public GameState State { get; private set; }

   public IReadOnlyList<RoundId> RoundIds => this.roundIds.AsReadOnly();

   private Round LastRound => this.rounds.Last() ?? new NoRound();

   public IReadOnlyCollection<GamePlayer> FindTeamPlayers(TeamId teamId)
      => this.teamPlayers.Where(p => p.TeamId == teamId)
         .ToList().AsReadOnly();

   public IReadOnlyCollection<GamePlayer> FindTeamPlayers()
      => this.teamPlayers.ToList().AsReadOnly();
}

public record CurrentRound : ValueObject
{
   private readonly Round currentRound;

   public CurrentRound(Round currentRound)
   {
      this.currentRound = currentRound;
   }

   public IReadOnlyList<PlayerDeal> PlayerDeals => this.currentRound.PlayerDeals;

   public PlayerId CurrentPlayerId => this.currentRound.CurrentPlayerId;

   public RoundId Id => this.currentRound.Id;

   public Round.RoundState State => this.currentRound.State;

   public IReadOnlyList<Bid> Bids => this.currentRound.Bids;

   public IReadOnlyList<TeamPlayer> TeamPlayers => this.currentRound.TeamPlayers;
}