// 🃏 The HossGame 🃏
// <copyright file="APlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate;

#region

using Hoss.Core.PlayerAggregate.Events;

#endregion

public sealed class Profile : Base
{
    private Profile(ProfileId id)
        : base(id)
    {
    }

    private PlayerState State { get; set; } = PlayerState.NotPlaying;

    public ProfileEmailBase Email { get; private set; } = new NoProfileEmail();

    public NameBase Name { get; private set; } = new NoName();

    public static Profile FromNewRegister(ProfileEmail profileEmail, PlayerName playerName)
    {
        var id = new AProfileId();
        var player = new Profile(id);
        player.Apply(new PlayerRegisteredEvent(id, profileEmail, playerName));

        return player;
    }

    public void RequestJoinGame(ValueId gameId)
    {
        /*This should be checked both by the client before sending the command and by a domain service after hydrating the player aggregate and before calling this method. There is a race condition where a player can join a game and tries to join another game after the condition is checked. Allowing a player to briefly join two games. This edge and rare case could be solved by:
        - Defining an SLA (?)
        - Having a separate process that checks whether an user has two games and doing some compensation
        - Decoupling the request event from the actual joining to a game. Allowing some time to catch on. Or by sending the id of the last event used to perform the check? */

        // Preconditions
        if (this.CanJoinNewGame)
            this.Apply(new RequestedJoinGameEvent(this.Id, gameId));
        else
            this.Apply(new CannotJoinGameEvent(this.Id, "APlayer already in a game"));

        this.EnsureValidState(); // Invariants
    }

    protected override void EnsureValidState()
    {
#pragma warning disable CS8509
        var valid = this.State switch
#pragma warning restore CS8509
        {
            PlayerState.Playing => this.JoiningGameId is not NoValueId,
            _ => true,
        };

        if (!valid) throw new InvalidEntityStateException($"Failed to validate entity {nameof(Profile)}");
    }

    protected override void When(DomainEventBase @event)
    {
        (@event switch
        {
            // CannotJoinGameEvent => () => { },
            PlayerRegisteredEvent e => (Action) (() => this.HandlePlayerRegisteredEvent(e)),
            RequestedJoinGameEvent e => () =>
            {
                this.State = PlayerState.Playing;
                this.JoiningGameId = e.GameId;
            },
            _ => () => { },
        }).Invoke();
    }

    private void HandlePlayerRegisteredEvent(PlayerRegisteredEvent @event)
    {
        this.Name = @event.PlayerName;
        this.Email = @event.Email;
    }

    #region Nested type: PlayerState

    private enum PlayerState
    {
        Playing,
        NotPlaying,
    }

    #endregion
}

public record AProfileId : ProfileId
{
    /// <inheritdoc />
    public override string ToString()
    {
        return this.Id.ToString();
    }
}

public record NoProfileId : ProfileId;

public abstract record ProfileId : ValueId;

public sealed class NoBase : Base
{
    /// <inheritdoc />
    public NoBase(ProfileId id)
        : base(id)
    {
        this.EnsureValidState();
    }

    /// <inheritdoc />
    protected override void When(DomainEventBase @event)
    {
    }

    /// <inheritdoc />
    protected override void EnsureValidState()
    {
    }
}