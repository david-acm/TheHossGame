// 🃏 The HossGame 🃏
// <copyright file="RoundStartedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.SharedKernel;

public record RoundStartedEvent(GameId GameId,
   RoundId RoundId,
   ADeck Deck)
   : DomainEventBase(GameId)
{
}

public record PlayerCards
   : ValueObject
{

}