// 🃏 The HossGame 🃏
// <copyright file="APlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.ProfileAggregate.Events;

namespace Hoss.Core.ProfileAggregate;

#region



#endregion

public sealed class Profile : Base
{
    private Profile(ProfileId id)
        : base(id)
    {
    }

    private PlayerState State { get; set; } = PlayerState.NotPlaying;

    public static Profile FromNewRegister(ProfileEmail profileEmail, PlayerName playerName, AProfileId profileId)
    {
        var player = new Profile(profileId);
        player.Apply(new PlayerRegisteredEvent(profileId, profileEmail, playerName));

        return player;
    }

    public void RequestJoinGame(ValueId gameId)
    {
        /*This should be checked both by the client before sending the command and by a domain service after hydrating the player aggregate and before calling this method. There is a race condition where a player can join a game and tries to join another game after the condition is checked. Allowing a player to briefly join two games. This edge and rare case could be solved by:
        - Defining an SLA (?)
        - Having a separate process that checks whether an user has two games and doing some compensation
        - Decoupling the request event from the actual joining to a game. Allowing some time to catch on. Or by sending the id of the last event used to perform the check? */

        // Preconditions
        if (CanJoinNewGame)
            Apply(new RequestedJoinGameEvent(Id, gameId));
        else
            Apply(new CannotJoinGameEvent(Id, "APlayer already in a game"));

        EnsureValidState(); // Invariants
    }

    protected override void EnsureValidState()
    {
#pragma warning disable CS8509
        var valid = State switch
#pragma warning restore CS8509
        {
            PlayerState.Playing => JoiningGameId is not NoValueId,
            _ => true,
        };

        if (!valid) throw new InvalidEntityStateException($"Failed to validate entity {nameof(Profile)}");
    }

    protected override void When(DomainEventBase @event)
    {
        (@event switch
        {
            // CannotJoinGameEvent => () => { },
            PlayerRegisteredEvent e => (Action)(() => HandlePlayerRegisteredEvent()),
            RequestedJoinGameEvent e => () =>
            {
                State = PlayerState.Playing;
                JoiningGameId = e.GameId;
            },
            _ => () => { },
        }).Invoke();
    }

    private void HandlePlayerRegisteredEvent()
    {
    }

    #region Nested type: PlayerState

    private enum PlayerState
    {
        Playing,
        NotPlaying,
    }

    #endregion
}

public record AProfileId(Guid? ValueId = null) : ProfileId(ValueId)
{
}

public record NoProfileId : ProfileId;

public record ProfileId(Guid? Id = null)  : ValueId
{
    public static implicit operator ProfileId(Guid s) => new(s);
}

public sealed class NoBase : Base
{
    /// <inheritdoc />
    public NoBase(ProfileId id)
        : base(id)
    {
        EnsureValidState();
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