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

using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

namespace Hoss.Core.GameAggregate.RoundEntity.RoundScoreValueObject;

public record RoundScore
{
    private RoundScore(TeamRoundScore team1, TeamRoundScore team2)
    {
        this.Team1 = team1;
        this.Team2 = team2;
    }

    public TeamRoundScore Team1 { get; }
    public TeamRoundScore Team2 { get; }

    internal static RoundScore FromRound((RoundPlayer, RoundPlayer) bidWinningTeam, Stack<Trick> tricks, Bid winningBid)
    {
        var tricksWonByBiddingTeam = tricks.Count(t => t.Winner == bidWinningTeam.Item1.PlayerId ||
                                                       t.Winner == bidWinningTeam.Item2.PlayerId);
        var winningTeamId = bidWinningTeam.Item1.TeamId;

        var score = CalculateScore(winningBid, tricksWonByBiddingTeam);

        return new RoundScore(
            new TeamRoundScore(winningTeamId, score),
            new TeamRoundScore(winningTeamId == TeamId.NorthSouth ? TeamId.EastWest : TeamId.NorthSouth, 0));
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
        return new RoundScore(new TeamRoundScore(TeamId.NorthSouth, 0), new TeamRoundScore(TeamId.EastWest, 0));
    }

    public static RoundScore operator +(RoundScore a, RoundScore b)
    {
        return new RoundScore(a.Team1 + b.Team1, a.Team2 + b.Team2);
    }

    public static implicit operator int(RoundScore score)
    {
        return Math.Max(score.Team1, score.Team2);
    }
}