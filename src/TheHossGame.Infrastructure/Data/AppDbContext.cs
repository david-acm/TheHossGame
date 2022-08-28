// 🃏 The HossGame 🃏
// <copyright file="AppDbContext.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure.Data;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class AppDbContext : DbContext
{
   private readonly IDomainEventDispatcher dispatcher;

   public AppDbContext(
       DbContextOptions<AppDbContext> options,
       IDomainEventDispatcher dispatcher)
       : base(options)
   {
      this.dispatcher = dispatcher;
   }

   public DbSet<ToDoItem> ToDoItems => this.Set<ToDoItem>();

   public DbSet<Project> Projects => this.Set<Project>();

   public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
   {
      int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

      // ignore events if no dispatcher provided
      if (this.dispatcher == null)
      {
         return result;
      }

      // dispatch events only if save was successful
      var entitiesWithEvents = this.ChangeTracker.Entries<EntityBase>()
          .Select(e => e.Entity)
          .Where(e => e.DomainEvents.Any())
          .ToArray();

      await this.dispatcher.DispatchAndClearEvents(entitiesWithEvents);

      return result;
   }

   public override int SaveChanges()
   {
      return this.SaveChangesAsync().GetAwaiter().GetResult();
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      Guard.Against.Null(modelBuilder);
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
   }
}
