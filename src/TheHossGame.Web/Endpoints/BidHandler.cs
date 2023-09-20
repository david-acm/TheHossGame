using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity.BidValueObject;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class BidHandler : UpdateHandler<BidRequest, AGame>
{
  public BidHandler(IAggregateStore store) : base(store){}

  public override void Configure() => Post("/{Id}/bids");

  protected override void Action(AGame aggregate, BidRequest req) =>
    aggregate.Bid(
      new APlayerId(req.PlayerId),
      BidValue.FromValue(req.bidValue)
    );
}

public sealed class TrumpSelectionHandler : UpdateHandler<TrumpSelectionRequest, AGame>
{
  public TrumpSelectionHandler(IAggregateStore store) : base(store){}

  public override void Configure() => Post("/{Id}/trumpSelections");

  protected override void Action(AGame aggregate, TrumpSelectionRequest req) =>
    aggregate.SelectTrump(
      new APlayerId(req.PlayerId),
      Suit.FromValue(req.trumpSuit)
    );
}

public record TrumpSelectionRequest(Guid PlayerId, char trumpSuit) : Command;

public record CardPlayRequest(Guid PlayerId, string Rank, char Suit) : Command;
