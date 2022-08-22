// 🃏 The HossGame 🃏
// <copyright file="ToDoItemCompletedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Events;

using TheHossGame.SharedKernel;

public class ToDoItemCompletedEvent : DomainEventBase
{
    public ToDoItemCompletedEvent(ToDoItem completedItem)
    {
        this.CompletedItem = completedItem;
    }

    public ToDoItem CompletedItem { get; set; }
}
