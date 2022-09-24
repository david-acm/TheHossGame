// 🃏 The HossGame 🃏
// <copyright file="NewItemAddedEvent.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Events;

using TheHossGame.SharedKernel;

public record NewItemAddedEvent(
        Project project,
        ToDoItem newItem)
   : DomainEventBase(project.Id)
{
}
