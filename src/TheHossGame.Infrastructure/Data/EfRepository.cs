// 🃏 The HossGame 🃏
// <copyright file="EfRepository.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure.Data;

using Ardalis.Specification.EntityFrameworkCore;
using TheHossGame.SharedKernel.Interfaces;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext)
        : base(dbContext)
  {
  }
}
