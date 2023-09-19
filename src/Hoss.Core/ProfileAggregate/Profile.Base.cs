// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.ProfileAggregate;

#region



#endregion

public abstract class Base : AggregateRoot
{
    protected Base(ProfileId id)
        : base(id.Id ?? Guid.Empty)
    {
        JoiningGameId = new NoValueId();
    }

    protected bool CanJoinNewGame => JoiningGameId is NoValueId;

    protected ValueId JoiningGameId { get; set; }
}