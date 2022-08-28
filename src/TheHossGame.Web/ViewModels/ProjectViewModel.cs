// 🃏 The HossGame 🃏
// <copyright file="ProjectViewModel.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.ViewModels;

using System.Collections.Generic;

public class ProjectViewModel
{
    private readonly List<ToDoItemViewModel> items;

    public ProjectViewModel(IList<ToDoItemViewModel> items)
    {
        this.items = items.ToList();
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    public IReadOnlyList<ToDoItemViewModel> Items => this.items.AsReadOnly();
}
