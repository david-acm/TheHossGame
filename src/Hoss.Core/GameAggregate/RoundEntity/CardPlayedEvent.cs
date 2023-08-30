// 🃏 The HossGame 🃏
// <copyright file="CardPlayedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

using Ardalis.GuardClauses;
using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;

public record CardPlayedEvent(GameId GameId, RoundId RoundId, PlayerId PlayerId, Card Card) : RoundEventBase(GameId,
    RoundId);

public record TrickPlayedEvent(GameId GameId, RoundId RoundId, CardPlay HandWinner) : RoundEventBase(GameId, RoundId);

public record RoundPlayedEvent(GameId GameId, RoundId RoundId, RoundScore RoundScore) : RoundEventBase(GameId, RoundId);

public record RoundScore
{
    private RoundScore(TeamRoundScore team1, TeamRoundScore team2)
    {
        this.team1 = team1;
        this.team2 = team2;
    }

    public TeamRoundScore team1 { get; }
    public TeamRoundScore team2 { get; }

    internal static RoundScore FromRound((RoundPlayer, RoundPlayer) bidWinningTeam, Stack<Trick> tricks, Bid winningBid)
    {
        var tricksWonByBiddingTeam = tricks.Count(t => t.Winner == bidWinningTeam.Item1.PlayerId ||
                                                       t.Winner == bidWinningTeam.Item2.PlayerId);
        var winningTeamId = bidWinningTeam.Item1.TeamId;
        int score;
        if (tricksWonByBiddingTeam >= winningBid.Value)
            score = tricksWonByBiddingTeam;
        else
            score = -winningBid.Value;
        return new RoundScore(
            new TeamRoundScore(winningTeamId, score),
            new TeamRoundScore(winningTeamId == Game.TeamId.Team1 ? Game.TeamId.Team2 : Game.TeamId.Team1, 0));
    }

    internal static RoundScore New()
    {
        return new RoundScore(new TeamRoundScore(Game.TeamId.Team1, 0), new TeamRoundScore(Game.TeamId.Team2, 0));
    }

    public static RoundScore operator +(RoundScore a, RoundScore b)
    {
        return new RoundScore(a.team1 + b.team1, a.team2 + b.team2);
    }

    public static implicit operator int(RoundScore score)
    {
        return Math.Max(score.team1, score.team2);
    }
}

public record TeamRoundScore
{
    private readonly Game.TeamId id;

    internal TeamRoundScore(Game.TeamId id, int score)
    {
        this.id = id;
        Guard.Against.OutOfRange(score, nameof(score), -24, 24);
        this.Score = score;
    }

    public int Score { get; }

    public static TeamRoundScore operator +(TeamRoundScore a, TeamRoundScore b)
    {
        if (a.id != b.id)
            throw new ArithmeticException("Cannot add scores for different teams");
        return new TeamRoundScore(a.id, a.Score + b.Score);
    }

    public static implicit operator int(TeamRoundScore roundScore)
    {
        return roundScore.Score;
    }
}

public record GameScore
{
    private GameScore(TeamGameScore team1, TeamGameScore team2)
    {
        this.team1Score = team1;
        this.team2Score = team2;
    }

    public TeamGameScore team1Score { get; }
    public TeamGameScore team2Score { get; }

    internal GameScore AddRoundScore(RoundScore roundScores)
    {
        return new GameScore(this.team1Score.AddTeamRoundScore(roundScores.team1),
            this.team2Score.AddTeamRoundScore(roundScores.team2));
    }

    internal static GameScore New()
    {
        return new GameScore(
            TeamGameScore.New(Game.TeamId.Team1),
            TeamGameScore.New(Game.TeamId.Team2));
    }

    public static implicit operator int(GameScore score)
    {
        return Math.Max(score.team1Score, score.team2Score);
    }
}

public record TeamGameScore
{
    private readonly Game.TeamId id;

    private TeamGameScore(Game.TeamId id, int score)
    {
        this.id = id;
        Guard.Against.OutOfRange(score, nameof(score), 0, 54);
        this.Score = score;
    }

    public int Score { get; }

    public TeamGameScore AddTeamRoundScore(TeamRoundScore roundScore)
    {
        return new TeamGameScore(this.id, this + roundScore);
    }

    public static TeamGameScore operator +(TeamGameScore a, TeamGameScore b)
    {
        if (a.id != b.id)
            throw new ArithmeticException("Cannot add scores for different teams");
        return new TeamGameScore(a.id, a.Score + b.Score);
    }

    public static TeamGameScore New(Game.TeamId id)
    {
        return new TeamGameScore(id, 0);
    }

    public static implicit operator int(TeamGameScore score)
    {
        return score.Score;
    }
}