// 🃏 The HossGame 🃏
// <copyright file="AGame.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.GameAggregate.PlayerEntity;
using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.PlayerAggregate;

/// <summary>
///    State side.
/// </summary>
public sealed partial class AGame : Game
{
   #region GameState enum

   public enum GameState
   {
      Created,
      TeamsFormed,
      Started,
      Finished,
   }

   #endregion

   private readonly List<GamePlayer> gamePlayers = new ();

   private readonly List<Round> rounds = new List<ARound>().Cast<Round>().ToList();

   private AGame(IShufflingService shufflingService)
      : base(new AGameId())
   {
      this.shufflingService = shufflingService;
   }

   public CurrentRound CurrentRound => new (this.NewestRound);

   public PlayerId CurrentPlayerId => this.CurrentRound.CurrentPlayerId;

   public GameState State { get; private set; }

   private Round NewestRound => this.rounds.LastOrDefault() ?? new NoRound();

   public IReadOnlyCollection<GamePlayer> FindGamePlayers(TeamId teamId)
   {
      return this.gamePlayers.Where(p => p.TeamId == teamId)
                 .ToList().AsReadOnly();
   }

   public IReadOnlyCollection<GamePlayer> FindTeamPlayers()
   {
      return this.gamePlayers.ToList().AsReadOnly();
   }

   public GamePlayer FindPlayer(PlayerId playerId)
   {
      return this.FindTeamPlayers().FirstOrDefault(p => p.PlayerId == playerId) ??
         new NoGamePlayer(
            this.Id,
            playerId,
            this.Apply);
   }
}
