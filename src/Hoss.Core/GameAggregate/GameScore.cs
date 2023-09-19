// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameScore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

using RoundEntity.RoundScoreValueObject;

public record GameScore
{
    private GameScore(TeamGameScore team1, TeamGameScore team2)
    {
        Team1Score = team1;
        Team2Score = team2;
    }

    public TeamGameScore Team1Score { get; }
    public TeamGameScore Team2Score { get; }

    internal GameScore AddRoundScore(RoundScore roundScores)
    {
        return new GameScore(Team1Score.AddTeamRoundScore(roundScores.Team1),
            Team2Score.AddTeamRoundScore(roundScores.Team2));
    }

    internal static GameScore New()
    {
        return new GameScore(
            TeamGameScore.New(TeamId.NorthSouth),
            TeamGameScore.New(TeamId.EastWest));
    }

    public static implicit operator int(GameScore score)
    {
        return Math.Max(score.Team1Score, score.Team2Score);
    }
}