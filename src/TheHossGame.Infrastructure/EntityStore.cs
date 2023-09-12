// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityStore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.Infrastructure;

using System.Text.Json;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static System.Text.Encoding;

public class EntityStore : IAggregateStore
{
    #region IAggregateStore Members

    /// <inheritdoc />
    public Task<T> LoadAsync<T, TId>(TId id)
        where TId : ValueId
        where T : AggregateRoot<TId>
    {
        return null;
    }

    /// <inheritdoc />
    public Task<bool> Exists<T, TId>(TId id)
        where TId : ValueId
        where T : AggregateRoot<TId>
    {
        return null;
    }

    /// <inheritdoc />
    public Task Save<T, TId>(T aggregate)
        where TId : ValueId
        where T : AggregateRoot<TId>
    {
        var events = aggregate.Events.Select(@event => new EventData());
        return null;
    }

    #endregion

    private static string GetStreamName<T, TId>(TId id)
        where TId : ValueId
        where T : AggregateRoot<TId>
    {
        return $"{typeof(T).Name}_{id}";
    }


    private static string GetStreamName<T, TId>(T aggregate)
        where TId : ValueId
        where T : AggregateRoot<TId>
    {
        return $"{typeof(T).Name}_{aggregate.Id}";
    }

    private static byte[] Serialize(object data)
    {
        return UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}