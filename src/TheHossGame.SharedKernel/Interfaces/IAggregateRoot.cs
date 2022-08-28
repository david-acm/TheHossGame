// ---
// 🃏 The HossGame 🃏
// <copyright file="IAggregateRoot.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel.Interfaces;

/// <summary>
/// Apply this marker interface only to aggregate root entities
/// Repositories will only work with aggregate roots, not their children.
/// </summary>
public interface IAggregateRoot
{
}
