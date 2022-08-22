// 🃏 The HossGame 🃏
// <copyright file="NewItemAddedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Events;

using TheHossGame.SharedKernel;

public class NewItemAddedEvent : DomainEventBase
{
  public NewItemAddedEvent(
      Project project,
      ToDoItem newItem)
  {
    this.Project = project;
    this.NewItem = newItem;
  }

  public ToDoItem NewItem { get; set; }

  public Project Project { get; set; }
}
