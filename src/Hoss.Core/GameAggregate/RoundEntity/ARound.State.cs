// 🃏 The HossGame 🃏
// <copyright file="ARound.State.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

   #region

using Hoss.Core.GameAggregate.Events;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;

#endregion

/// <summary>
///    The state side.
/// </summary>
public sealed partial class ARound : Round
{
   private readonly Stack<CardPlay> tableCenter = new ();
   private List<Bid> bids = new ();
   private List<ADeal> deals = new ();
   private RoundState state;
   private Queue<RoundPlayer> teamPlayers = new ();
   private TrumpSelection trumpSelection = new (new NoPlayerId(), Suit.None);

   private ARound(GameId gameId, IEnumerable<RoundPlayer> teamPlayers, Action<DomainEventBase> when)
      : this(gameId, new RoundId(), when)
   {
      this.OrderPlayers(teamPlayers);
   }

   private ARound(GameId gameId, RoundId roundId, Action<DomainEventBase> when)
      : base(roundId, when)
   {
      this.GameId = gameId;
   }

   internal override RoundState State => this.state;

   internal override IReadOnlyList<ADeal> Deals => this.deals.AsReadOnly();

   internal override IReadOnlyList<RoundPlayer> RoundPlayers => this.teamPlayers.ToList().AsReadOnly();

   internal override IReadOnlyList<Bid> Bids => this.bids.AsReadOnly();

   /// <inheritdoc />
   internal override IReadOnlyList<CardPlay> CardsPlayed => this.tableCenter.ToList().AsReadOnly();

   internal override PlayerId CurrentPlayerId => this.teamPlayers.Peek().PlayerId;

   internal override Suit SelectedTrump => this.trumpSelection.Suit;

   private GameId GameId { get; }

   internal override ADeal DealForPlayer(PlayerId playerId)
   {
      return this.deals.First(d => d.PlayerId == playerId);
   }

   internal override IEnumerable<Card> CardsForPlayer(PlayerId playerId) => this.deals.First(d => d.PlayerId == playerId).Cards;

   /// <inheritdoc />
   protected override void Apply(DomainEventBase @event)
   {
      var roundEvent = @event as RoundEventBase;
      this.EnsurePreconditions(roundEvent!);
      base.Apply(@event);
   }
}
