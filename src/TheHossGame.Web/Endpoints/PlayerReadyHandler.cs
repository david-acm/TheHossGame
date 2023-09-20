using Hoss.Core.GameAggregate;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class PlayerReadyHandler : UpdateHandler<PlayerReadyRequest, AGame>
{
  public PlayerReadyHandler(IAggregateStore store) : base(store){}

  public override void Configure() => Put("/{Id}/playerReady");

  protected override void Action(AGame aggregate, PlayerReadyRequest req)
  {
    aggregate.TeamPlayerReady(
      new APlayerId(req.PlayerId));
  }
}

public record BidRequest(Guid PlayerId, int bidValue) : Command;