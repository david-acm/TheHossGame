using FastEndpoints;
using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class JoinGameHandler : UpdateHandler<JoinGameRequest, AGame, AGameId>
{
  public JoinGameHandler(IAggregateStore store) : base(store) {}

  public override void Configure() => Put("/joinRequests");

  protected override void Action(AGame aggregate, JoinGameRequest req)
  {
    aggregate.JoinPlayerToTeam(
      new APlayerId(req.PlayerId),
      req.TeamId);
  }
}

public interface IHandleCommand<TCommand,TId>
  where TCommand : Command<TId>
  where TId : ValueId
{
  Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public abstract record Command<TId>
{
  public Guid Id { get; set; }
}

public abstract class UpdateHandler<T, TAggregate, TId> : Endpoint<T>
  where T : Command<TId>
  where TAggregate : AggregateRoot, IAggregateRoot
  where TId : ValueId
{
  private readonly IAggregateStore store;

  protected UpdateHandler(IAggregateStore store)
  {
    this.store = store;
  }

  public override async Task HandleAsync(T req, CancellationToken ct)
  {
    var aggregate = await LoadAsync(req.Id);
    Action(aggregate, req);
    await SaveAsync(aggregate);
  }

  protected abstract void Action(TAggregate aggregate, T command);

  private async Task SaveAsync(TAggregate aggregate)
  {
    await store.SaveAsync(aggregate);
  }

  public async Task<TAggregate> LoadAsync(Guid id) => await store.LoadAsync<TAggregate>(id);
}