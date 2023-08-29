// 🃏 The HossGame 🃏
// <copyright file="ToDoConfiguration.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace Hoss.Infrastructure.Data.Config;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
   public void Configure(EntityTypeBuilder<ToDoItem> builder)
   {
      Guard.Against.Null(builder);

      builder
         .HasKey(p => p.IdValue);
      builder
         .Ignore(p => p.Id);
      builder.Property(t => t.Title)
          .IsRequired();
   }
}
