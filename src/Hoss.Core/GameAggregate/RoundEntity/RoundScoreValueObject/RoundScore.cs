// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundScore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.RoundScoreValueObject;

using Hoss.Core.GameAggregate.RoundEntity.BidEntity;

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

        var score = CalculateScore(winningBid, tricksWonByBiddingTeam);

        return new RoundScore(
            new TeamRoundScore(winningTeamId, score),
            new TeamRoundScore(winningTeamId == Game.TeamId.Team1 ? Game.TeamId.Team2 : Game.TeamId.Team1, 0));
    }

    private static int CalculateScore(Bid winningBid, int tricksWonByBiddingTeam)
    {
        int score;
        if (tricksWonByBiddingTeam >= winningBid.Value || winningBid.Value == BidValue.Hoss)
            score = CalculatePositiveScore(winningBid, tricksWonByBiddingTeam);
        else
            score = -winningBid.Value;
        return score;
    }

    private static int CalculatePositiveScore(Bid winningBid, int tricksWonByBiddingTeam)
    {
        return winningBid.Value == BidValue.Hoss ? winningBid.Value : tricksWonByBiddingTeam;
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