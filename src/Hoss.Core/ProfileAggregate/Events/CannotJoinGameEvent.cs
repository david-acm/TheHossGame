// 🃏 The HossGame 🃏
// <copyright file="CannotJoinGameEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.ProfileAggregate.Events;

#region

#endregion

public record CannotJoinGameEvent(ProfileId ProfileId, string Reason) : ProfileEventBase(ProfileId);

public abstract record ProfileEventBase : DomainEventBase
{
    protected ProfileEventBase(ProfileId profileId)
        : base()
    {
        ProfileId = profileId;
    }

    public ProfileId ProfileId { get; }
}