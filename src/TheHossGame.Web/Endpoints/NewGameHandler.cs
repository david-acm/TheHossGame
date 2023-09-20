using System.ComponentModel.DataAnnotations;
using Hoss.Core.GameAggregate;
using Hoss.Core.Interfaces;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class NewGameHandler : CreateHandler<CreateGameRequest, AGame>
{

  public NewGameHandler(IAggregateStore aggregateStore)
  : base(aggregateStore)
  {
  }

  public override void Configure() => Post("/games");
  protected override AGame Action(CreateGameRequest command) => AGame.CreateForPlayer(command.GameId, command.PlayerId, new ShufflingService(new RandomNumberProvider()));
}

public record CreateGameRequest : Command
{
  [Required] public Guid PlayerId { get; set; }

  [Required] public Guid GameId { get; set; }
}

public class CreateGameResponse
{
}

public record JoinGameRequest : Command
{
  public Guid PlayerId { get; set; }
  
  public TeamId TeamId { get; set; }
}