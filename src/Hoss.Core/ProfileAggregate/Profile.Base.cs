// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate;

#region

using Hoss.SharedKernel.Interfaces;

#endregion

public abstract class Base : AggregateRoot<ProfileId>, IAggregateRoot
{
    protected Base(ProfileId id)
        : base(id)
    {
        this.JoiningGameId = new NoValueId();
    }

    protected bool CanJoinNewGame => this.JoiningGameId is NoValueId;

    protected ValueId JoiningGameId { get; set; }
}