// ---
// 🃏 The HossGame 🃏
// <copyright file="IReadRepository.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel.Interfaces;

using Ardalis.Specification;

/// <summary>
///    A readonly repository for AggregateRoots.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T>
   where T : class, IAggregateRoot
{
}
