// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.Events;

using TheHossGame.Core.GameAggregate.RoundEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static Game;

public record PlayerJoinedEvent(GameId GameId, PlayerId PlayerId, TeamId TeamId)
   : DomainEventBase(PlayerId);

public record PlayerAlreadyInGameEvent(PlayerId PlayerId)
   : DomainEventBase(PlayerId);

public record TeamsFormedEvent(GameId GameId)
   : DomainEventBase(GameId);

public record PlayerReadyEvent(GameId GameId, PlayerId PlayerId)
   : DomainEventBase(GameId);

public record GameStartedEvent(
      GameId GameId,
      RoundId RoundId,
      IEnumerable<RoundPlayer> TeamPlayers,
      IEnumerable<PlayerDeal> Deals,
      IEnumerable<Bid> Bids)
   : DomainEventBase(GameId);

public record GameFinishedEvent(GameId GameId)
   : DomainEventBase(GameId);
