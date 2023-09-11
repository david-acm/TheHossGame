// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TeamGameScore.cs" company="Microsoft">
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

using Ardalis.GuardClauses;
using Hoss.Core.GameAggregate.RoundEntity.RoundScoreValueObject;

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