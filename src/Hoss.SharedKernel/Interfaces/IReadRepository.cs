// 🃏 The HossGame 🃏
// <copyright file="IReadRepository.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel.Interfaces;

#region

using Ardalis.Specification;

#endregion

/// <summary>
///    A readonly repository for AggregateRoots.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T>
   where T : class, IAggregateRoot
{
}
