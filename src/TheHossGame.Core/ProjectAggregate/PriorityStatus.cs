// 🃏 The HossGame 🃏
// <copyright file="PriorityStatus.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate;

using Ardalis.SmartEnum;

public class PriorityStatus : SmartEnum<PriorityStatus>
{
    public static readonly PriorityStatus Backlog = new (nameof(Backlog), 0);
    public static readonly PriorityStatus Critical = new (nameof(Critical), 1);

    private PriorityStatus(string name, int value)
      : base(name, value)
    {
    }
}
