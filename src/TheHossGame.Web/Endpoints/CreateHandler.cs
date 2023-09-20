using FastEndpoints;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public abstract class CreateHandler<T, TAggregate> : Endpoint<T>
  where T : Command
  where TAggregate : AggregateRoot, IAggregateRoot
{
  private readonly IAggregateStore store;

  protected CreateHandler(IAggregateStore store) => this.store = store;

  public override async Task HandleAsync(T req, CancellationToken ct)
  {
    var aggregate = Action(req);
    await SaveAsync(aggregate);
  }

  protected abstract TAggregate Action( T command);

  private async Task SaveAsync(TAggregate aggregate) => await store.SaveAsync(aggregate);

  private async Task<TAggregate> LoadAsync(Guid id) => await store.LoadAsync<TAggregate>(id);
}