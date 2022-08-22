// 🃏 The HossGame 🃏
// <copyright file="ToDoConfiguration.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure.Data.Config;

using TheHossGame.Core.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ardalis.GuardClauses;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        Guard.Against.Null(builder);
        builder.Property(t => t.Title)
            .IsRequired();
    }
}
