// 🃏 The HossGame 🃏
// <copyright file="ProjectConfiguration.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure.Data.Config;

using TheHossGame.Core.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ardalis.GuardClauses;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        Guard.Against.Null(builder);
        builder.Property(p => p.Name)
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(p => p.Priority)
          .HasConversion(
              p => p.Value,
              p => PriorityStatus.FromValue(p));
    }
}
