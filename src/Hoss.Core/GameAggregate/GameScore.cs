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

using Hoss.Core.GameAggregate.RoundEntity.RoundScoreValueObject;

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