// 🃏 The HossGame 🃏
// <copyright file="RoundStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record RoundStartedEvent(GameId GameId,
   RoundId RoundId,
   ADeck Deck,
   IEnumerable<PlayerCards> PlayerCards)
   : DomainEventBase(GameId)
{
}

public record PlayerCards
   : ValueObject
{
   private readonly List<Card> cards = new List<Card>();

   public PlayerCards(PlayerId PlayerId)
   {
      this.PlayerId = PlayerId;
   }

   public PlayerId PlayerId { get; }
   public IReadOnlyList<Card> Cards => this.cards.AsReadOnly();

   public void ReceibeCard(Card cards)
   {
      this.cards.Add(cards);
   }
}