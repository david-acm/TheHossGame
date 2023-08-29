// 🃏 The HossGame 🃏
// <copyright file="ProjectConfiguration.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace Hoss.Infrastructure.Data.Config;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
   public void Configure(EntityTypeBuilder<Project> builder)
   {
      Guard.Against.Null(builder);

      builder
         .HasKey(p => p.IdValue);
      builder
         .Ignore(p => p.Id);

      builder.Property(p => p.Name)
      .HasMaxLength(100)
      .IsRequired();

      builder.Property(p => p.Priority)
        .HasConversion(
            p => p.Value,
            p => PriorityStatus.FromValue(p));
   }
}
