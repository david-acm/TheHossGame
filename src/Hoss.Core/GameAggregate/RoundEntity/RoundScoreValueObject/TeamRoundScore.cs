// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TeamRoundScore.cs" company="Microsoft">
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

using Ardalis.GuardClauses;

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