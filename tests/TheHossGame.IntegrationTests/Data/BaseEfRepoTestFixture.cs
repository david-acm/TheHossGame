// 🃏 The HossGame 🃏
// <copyright file="BaseEfRepoTestFixture.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.IntegrationTests.Data;

using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Infrastructure.Data;
using TheHossGame.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public abstract class BaseEfRepoTestFixture : IDisposable
{
  private readonly AppDbContext dbContext;
  private bool disposedValue;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var mockEventDispatcher = new Mock<IDomainEventDispatcher>();

    this.dbContext = new AppDbContext(options, mockEventDispatcher.Object);
  }

  protected EfRepository<Project> Repository => new (this.DbContext);

  protected AppDbContext DbContext => this.dbContext;

  public void Dispose()
  {
    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    this.Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!this.disposedValue)
    {
      if (disposing)
      {
        this.dbContext.Dispose();
      }

      this.disposedValue = true;
    }
  }
}
