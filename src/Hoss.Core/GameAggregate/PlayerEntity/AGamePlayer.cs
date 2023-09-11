// 🃏 The HossGame 🃏
// <copyright file="AGamePlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.PlayerEntity;

#region

using static Game;

#endregion

public class AGamePlayer : GamePlayer
{
    internal AGamePlayer(GameId gameId, PlayerId playerId, Action<DomainEventBase> applier)
        : base(gameId, playerId, applier)
    {
    }

    private bool HasJoinedGame => this.TeamId != TeamId.NoTeamId;

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
        if (this.HasJoinedGame)
        {
            this.Apply(new GameEvents.PlayerAlreadyInGameEvent(this.Id));
            return;
        }

        var @event = new GameEvents.PlayerJoinedEvent(this.GameId, this.Id, teamId);
        this.Apply(@event);
    }

    internal override void Ready()
    {
        var @event = new GameEvents.PlayerReadyEvent(this.GameId, this.Id);
        this.Apply(@event);
    }

    protected override void EnsureValidState()
    {
    }

    protected override void When(DomainEventBase @event)
    {
        switch (@event)
        {
            case GameEvents.PlayerJoinedEvent(_, var playerId, var teamId):
                this.PlayerId = playerId;
                this.TeamId = teamId;
                break;
            case GameEvents.PlayerReadyEvent:
                this.IsReady = true;
                break;
        }
    }
}