// 🃏 The HossGame 🃏
// <copyright file="ToDoItemViewModel.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.ViewModels;

using Ardalis.GuardClauses;
using TheHossGame.Core.ProjectAggregate;

public class ToDoItemViewModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsDone { get; private set; }

    public static ToDoItemViewModel FromToDoItem(ToDoItem item)
    {
        Guard.Against.Null(item);
        return new ToDoItemViewModel()
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsDone = item.IsDone,
        };
    }
}
