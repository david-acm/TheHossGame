// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.Events;

#region

using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;
using static Game;

#endregion

public record PlayerJoinedEvent(GameId GameId, PlayerId PlayerId, TeamId TeamId) : GameEventBase(GameId);

public record PlayerAlreadyInGameEvent(PlayerId PlayerId) : DomainEventBase(PlayerId);

public record TeamsFormedEvent(GameId GameId) : GameEventBase(GameId);

public record PlayerReadyEvent(GameId GameId, PlayerId PlayerId) : DomainEventBase(GameId);

public record GameStartedEvent(GameId GameId, RoundId RoundId, IEnumerable<RoundPlayer> TeamPlayers,
    IEnumerable<ADeal> Deals, IEnumerable<Bid> Bids) : GameEventBase(GameId);

public record GameFinishedEvent(GameId GameId) : GameEventBase(GameId);

public record RoundPlayedEvent(GameId GameId) : GameEventBase(GameId);