// 🃏 The HossGame 🃏
// <copyright file="PlayerJoinedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

#region

using Hoss.Core.GameAggregate.RoundEntity;
using Hoss.Core.GameAggregate.RoundEntity.BidEntity;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using static Game;

#endregion

public static class GameEvents
{
    #region Nested type: GameEventBase

    public abstract record GameEventBase(GameId GameId) : DomainEventBase(GameId);

    #endregion

    #region Nested type: GameFinishedEvent

    public record GameFinishedEvent(GameId GameId) : GameEventBase(GameId);

    #endregion

    #region Nested type: GameStartedEvent

    public record GameStartedEvent(GameId GameId, RoundId RoundId, IEnumerable<RoundPlayer> TeamPlayers,
        IEnumerable<ADeal> Deals, IEnumerable<Bid> Bids) : GameEventBase(GameId);

    #endregion

    #region Nested type: NewGameCreatedEvent

    public record NewGameCreatedEvent(GameId GameId, PlayerId StartedBy) : GameEventBase(GameId);

    #endregion

    #region Nested type: PlayerAlreadyInGameEvent

    public record PlayerAlreadyInGameEvent(PlayerId PlayerId) : DomainEventBase(PlayerId);

    #endregion

    #region Nested type: PlayerJoinedEvent

    public record PlayerJoinedEvent(GameId GameId, PlayerId PlayerId, TeamId TeamId) : GameEventBase(GameId);

    #endregion

    #region Nested type: PlayerReadyEvent

    public record PlayerReadyEvent(GameId GameId, PlayerId PlayerId) : DomainEventBase(GameId);

    #endregion

    #region Nested type: TeamsFormedEvent

    public record TeamsFormedEvent(GameId GameId) : GameEventBase(GameId);

    #endregion
}