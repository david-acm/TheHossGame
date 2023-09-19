// 🃏 The HossGame 🃏
// <copyright file="AGamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.PlayerEntity;

#region



#endregion

public class AGamePlayer : GamePlayer
{
    internal AGamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
        : base(gameId, playerId, applier)
    {
    }

    private bool HasJoinedGame => TeamId != TeamId.NoTeamId;

    internal static AGamePlayer FromStream(GameEvents.PlayerJoinedEvent @event, Action<DomainEventBase> applier)
    {
        var (gameId, playerId, teamId) = @event;
        return new AGamePlayer(gameId, playerId, applier)
        {
            TeamId = teamId,
        };
    }

    internal override void Join(TeamId teamId)
    {
        if (HasJoinedGame)
        {
            Apply(new GameEvents.PlayerAlreadyInGameEvent(Id));
            return;
        }

        var @event = new GameEvents.PlayerJoinedEvent(GameId, Id, teamId);
        Apply(@event);
    }

    internal override void Ready()
    {
        var @event = new GameEvents.PlayerReadyEvent(GameId, Id);
        Apply(@event);
    }

    protected override void EnsureValidState()
    {
    }

    protected override void When(DomainEventBase @event)
    {
        switch (@event)
        {
            case GameEvents.PlayerJoinedEvent(_, var playerId, var teamId):
                PlayerId = playerId;
                TeamId = teamId;
                break;
            case GameEvents.PlayerReadyEvent:
                IsReady = true;
                break;
        }
    }
}