// 🃏 The HossGame 🃏
// <copyright file="AGame.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

#region

using PlayerEntity;
using RoundEntity;
using Interfaces;

#endregion

/// <summary>
///    State side.
/// </summary>
public sealed partial class AGame : AggregateRoot
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

    private AGame(Guid id) : base(id)
    {
        shufflingService = new ShufflingService(
            new RandomNumberProvider());
    }
    
    private AGame(AGameId id, IShufflingService shufflingService)
        : base(id)
    {
        this.shufflingService = shufflingService;
    }

    public RoundView CurrentRoundView => new(CurrentRoundBase);

    public PlayerId CurrentPlayerId => CurrentRoundView.CurrentPlayerId;

    public GameState Stage { get; private set; }

    private RoundBase CurrentRoundBase => rounds.LastOrDefault() ?? new NoRound();
    public GameScore Score { get; private set; } = GameScore.New();

    public IReadOnlyCollection<GamePlayer> FindGamePlayers(TeamId teamId)
    {
        return gamePlayers.Where(p => p.TeamId == teamId).ToList().AsReadOnly();
    }

    public IReadOnlyCollection<GamePlayer> FindGamePlayers()
    {
        return gamePlayers.ToList().AsReadOnly();
    }

    public GamePlayer FindPlayer(PlayerId playerId)
    {
        return FindGamePlayers().FirstOrDefault(p => p.PlayerId.Id == playerId.Id) ??
               new NoGamePlayer(Id, playerId, Apply);
    }
}