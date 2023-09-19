using System.ComponentModel.DataAnnotations;
using FastEndpoints;
using Hoss.Core.GameAggregate;
using Hoss.Core.Interfaces;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class NewGameHandler : Endpoint<CreateGameRequest, CreateGameResponse>
{
  private readonly IAggregateStore aggregateStore;

  public NewGameHandler(IAggregateStore aggregateStore)
  {
    this.aggregateStore = aggregateStore;
  }

  public override void Configure()
  {
    Post("/games");
  }

  public override async Task HandleAsync(CreateGameRequest req, CancellationToken ct)
  {
    // TODO: Inject service, consider random number generator to be cryptographically signed, consider creating shuffling services for different development cases and production. 
    var shufflingService = new ShufflingService(new RandomNumberProvider());
    var game = AGame.CreateForPlayer(new AGameId(req.GameId), new APlayerId(req.PlayerId), shufflingService);
    await aggregateStore.SaveAsync(game);
  }
}

public record CreateGameRequest
{
  [Required] public Guid PlayerId { get; set; }

  [Required] public Guid GameId { get; set; }
}

public class CreateGameResponse
{
}

public record JoinGameRequest : Command<AGameId>
{
  public Guid PlayerId { get; set; }
  public Guid GameId { get; set; }
  
  public AGameId AGameId => new AGameId(GameId);
  public TeamId TeamId { get; set; }
}