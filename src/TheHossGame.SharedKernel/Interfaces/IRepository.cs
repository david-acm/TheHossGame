﻿// ---
// 🃏 The HossGame 🃏
// <copyright file="IRepository.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel.Interfaces;

using Ardalis.Specification;

// from Ardalis.Specification

/// <summary>
/// A write only repository for AggregateRoots.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}
