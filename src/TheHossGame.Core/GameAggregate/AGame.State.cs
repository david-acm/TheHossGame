﻿// 🃏 The HossGame 🃏
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

/// <summary>
/// State side.
/// </summary>
public partial class AGame : Game
{
   private readonly List<GamePlayer> gamePlayers = new ();
   private readonly List<RoundId> roundIds = new ();
   private readonly List<Round> rounds = new List<ARound>().Cast<Round>().ToList();

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

   private Round LastRound => this.rounds
      .FirstOrDefault(r => r.State == Round.RoundState.CardsDealt) ?? new NoRound();

   public IReadOnlyCollection<GamePlayer> FindGamePlayers(TeamId teamId)
      => this.gamePlayers.Where(p => p.TeamId == teamId)
         .ToList().AsReadOnly();

   public IReadOnlyCollection<GamePlayer> FindTeamPlayers()
      => this.gamePlayers.ToList().AsReadOnly();

   public GamePlayer FindPlayer(PlayerId playerId)
      => this.FindTeamPlayers().FirstOrDefault(p => p.PlayerId == playerId)
      ?? new NoGamePlayer(this.Id, playerId, this.Apply);
}