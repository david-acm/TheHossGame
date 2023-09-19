// 🃏 The HossGame 🃏
// <copyright file="NoGamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.PlayerEntity;

#region



#endregion

public sealed class NoGamePlayer : GamePlayer
{
    public NoGamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
        : base(gameId, playerId, applier)
    {
    }

    internal override void Join(TeamId teamId)
    {
        var player = new AGamePlayer(GameId, Id, Applier);
        player.Join(teamId);
        When(new GameEvents.PlayerJoinedEvent(GameId, Id, teamId));
        EnsureValidState();
    }

    internal override void Ready()
    {
    }

    protected override void EnsureValidState()
    {
    }

    protected override void When(DomainEventBase @event)
    {
    }
}