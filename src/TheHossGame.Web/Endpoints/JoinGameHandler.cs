using Hoss.Core.GameAggregate;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class JoinGameHandler : UpdateHandler<JoinGameRequest, AGame>
{
  public JoinGameHandler(IAggregateStore store) : base(store){}

  public override void Configure() => Put("/{Id}/joinRequests");

  protected override void Action(AGame aggregate, JoinGameRequest req)
  {
    aggregate.JoinPlayerToTeam(
      new APlayerId(req.PlayerId),
      req.TeamId);
  }
}

public record PlayerReadyRequest(Guid PlayerId) : Command;