﻿// 🃏 The HossGame 🃏
// <copyright file="IAggregateRoot.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel.Interfaces;

/// <summary>
///    Apply this marker interface only to aggregate root entities
///    Repositories will only work with aggregate roots, not their children.
/// </summary>
#pragma warning disable CA1040
public interface IAggregateRoot
#pragma warning restore CA1040
{
}