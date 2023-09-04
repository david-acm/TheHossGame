// 🃏 The HossGame 🃏
// <copyright file="AGame.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

#region

using Hoss.Core.GameAggregate.PlayerEntity;
using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.Interfaces;
using Hoss.Core.PlayerAggregate;

#endregion

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

    private const int MaxScore = 31;

    private readonly List<GamePlayer> gamePlayers = new();

    private readonly List<RoundBase> rounds = new List<Round>().Cast<RoundBase>().ToList();

    private AGame(IShufflingService shufflingService)
        : base(new AGameId())
    {
        this.shufflingService = shufflingService;
    }

    public RoundView CurrentRoundView => new(this.CurrentRoundBase);

    public PlayerId CurrentPlayerId => this.CurrentRoundView.CurrentPlayerId;

    public GameState Stage { get; private set; }

    private RoundBase CurrentRoundBase => this.rounds.LastOrDefault() ?? new NoRoundBase();
    public GameScore Score { get; private set; } = GameScore.New();

    public IReadOnlyCollection<GamePlayer> FindGamePlayers(TeamId teamId)
    {
        return this.gamePlayers.Where(p => p.TeamId == teamId).ToList().AsReadOnly();
    }

    public IReadOnlyCollection<GamePlayer> FindGamePlayers()
    {
        return this.gamePlayers.ToList().AsReadOnly();
    }

    public GamePlayer FindPlayer(PlayerId playerId)
    {
        return this.FindGamePlayers().FirstOrDefault(p => p.PlayerId == playerId) ??
               new NoGamePlayer(this.Id, playerId, this.Apply);
    }
}