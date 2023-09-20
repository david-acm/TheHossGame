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

using System.Reflection;
using EventStore.Client;
using static System.Text.Json.JsonSerializer;
using StreamPosition = EventStore.Client.StreamPosition;

namespace TheHossGame.Infrastructure;

using System.Text.Json;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;
using static System.Text.Encoding;

public class AggregateStore : IAggregateStore
{
  private readonly EventStoreClient connection;

  public AggregateStore(EventStoreClient connection)
  {
    this.connection = connection;
  }

  #region IAggregateStore Members

  /// <inheritdoc />
  public async Task<T> LoadAsync<T>(Guid id)
    where T : IAggregateRoot
  {
    var stream = GetStreamName<T>(id);
    // var aggregate = (T)CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null,
    //   new[] { new AGameId(id) })!;

    var aggregate = Construct<T>(new[]
    {
      typeof(Guid)
    }, new object[]
    {
      id
    });

    var page = await connection
      .ReadStreamAsync(
        Direction.Forwards,
        stream,
        0,
        1024).ToListAsync().ConfigureAwait(false);

    aggregate.Load(page.Select(@event =>
    {
      var readOnlyMemory = @event.Event.Metadata.Span;
      var meta = Deserialize<EventMetadata>(
        UTF8.GetString(readOnlyMemory));
      var dataType = Type.GetType(meta?.ClrType ?? string.Empty);

      var jsonData = UTF8.GetString(@event.Event.Data.Span);

      var data = (DomainEventBase)Deserialize(jsonData, dataType!)!;

      return data;
    }));

    return aggregate;
  }

  /// <inheritdoc />
  public async Task<bool> Exists<T>(Guid id)
    where T : IAggregateRoot
  {
    var stream = GetStreamName<T>(id);

    var result = await connection.ReadStreamAsync(
      Direction.Forwards,
      stream,
      StreamPosition.Start,
      1).ReadState.ConfigureAwait(false);

    return result != ReadState.StreamNotFound;
  }

  /// <inheritdoc />
  public async Task SaveAsync<T>(T aggregate)
    where T : IAggregateRoot
  {
    if (aggregate is null) return;

    var events = aggregate.Events
      .Select(@event => new EventData(
        Uuid.NewUuid(),
        type: @event.GetType().Name,
        data: Serialize(@event),
        metadata: Serialize(
          new EventMetadata(@event.GetType().AssemblyQualifiedName)))).ToList();

    if (events?.Any() != true)
      return;
    var streamName = GetStreamName<T>(aggregate.Id);
    var expectedRevision = new StreamRevision(aggregate.Version - 1);
    await connection
      .AppendToStreamAsync(
        streamName,
        expectedRevision == 0 ? StreamRevision.None : expectedRevision,
        events).ConfigureAwait(false);
  }

  #endregion

  private static string GetStreamName<T>(Guid id)
    where T : IAggregateRoot
  {
    return $"{typeof(T).Name}_{id}";
  }


  private static string GetStreamName<T>(T aggregate)
    where T : AggregateRoot
  {
    return $"{typeof(T).Name}_{aggregate.Id}";
  }

  private static byte[] Serialize(object data)
  {
    return UTF8.GetBytes(JsonSerializer.Serialize(data));
  }

  private static T Construct<T>(Type[] paramTypes, object[] paramValues)
  {
    var t = typeof(T);

    var ci = t.GetConstructor(
      BindingFlags.Instance | BindingFlags.NonPublic,
      null, paramTypes, null)!;

    return (T)ci.Invoke(paramValues);
  }
}

public record EventMetadata(string? ClrType);