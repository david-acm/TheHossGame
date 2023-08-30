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

public record RoundPlayedEvent(GameId GameId, RoundId RoundId, RoundScore Score) : RoundEventBase(GameId, RoundId);

public record RoundScore
{
    private RoundScore(TeamScore team1, TeamScore team2)
    {
        this.team1 = team1;
        this.team2 = team2;
    }

    public TeamScore team1 { get; }
    public TeamScore team2 { get; }


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
            new TeamScore(winningTeamId, score),
            new TeamScore(winningTeamId == Game.TeamId.Team1 ? Game.TeamId.Team1 : Game.TeamId.Team2, 0));
    }
}

public record TeamScore
{
    private readonly Game.TeamId id;

    internal TeamScore(Game.TeamId id, int score)
    {
        this.id = id;
        Guard.Against.OutOfRange(score, nameof(score), -24, 24);
        this.Score = score;
    }

    public int Score { get; }
}