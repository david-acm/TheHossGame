using Hoss.Core.GameAggregate;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

public sealed class CardPlaysHandler : UpdateHandler<CardPlayRequest, AGame>
{
  private readonly IAggregateStore store;

  public CardPlaysHandler(IAggregateStore store) : base(store)
  {
    this.store = store;
  }

  public override void Configure()
  {
    Post("/{Id}/CardPlays");
  }

  protected override void Action(AGame aggregate, CardPlayRequest req)
  {
    aggregate.PlayCard(
      new APlayerId(req.PlayerId),
      new ACard(
        Rank.FromValue(req.Rank),
        Suit.FromValue(req.Suit)));
  }
}