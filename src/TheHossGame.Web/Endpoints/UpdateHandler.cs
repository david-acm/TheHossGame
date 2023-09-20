using FastEndpoints;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public abstract class UpdateHandler<T, TAggregate> : Endpoint<T>
  where T : Command
  where TAggregate : AggregateRoot, IAggregateRoot
{
  private readonly IAggregateStore store;

  protected UpdateHandler(IAggregateStore store) => this.store = store;

  public override async Task HandleAsync(T req, CancellationToken ct)
  {
    var aggregate = await LoadAsync(req.Id);
    try
    {
      Action(aggregate, req);
    }
    catch (InvalidEntityStateException e)
    {
      await SendAsync(new
      {
        e.Message
      }, 400);
    }
    catch (Exception e)
    {
      await SendAsync(new
      {
        e.Message
      }, 500);
    }

    await SaveAsync(aggregate);
  }

  protected abstract void Action(TAggregate aggregate, T command);

  private async Task SaveAsync(TAggregate aggregate) => await store.SaveAsync(aggregate);

  private async Task<TAggregate> LoadAsync(Guid id) => await store.LoadAsync<TAggregate>(id);
}