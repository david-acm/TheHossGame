// 🃏 The HossGame 🃏
// <copyright file="Project.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate;

using Ardalis.GuardClauses;
using TheHossGame.Core.ProjectAggregate.Events;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public class Project : EntityBase, IAggregateRoot
{
   private readonly List<ToDoItem> items = new List<ToDoItem>();

   public Project(string name, PriorityStatus priority)
   {
      this.Name = Guard.Against.NullOrEmpty(name, nameof(name));
      this.Priority = priority;
   }

   public string Name { get; private set; }

   public IEnumerable<ToDoItem> Items => this.items.AsReadOnly();

   public ProjectStatus Status => this.items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;

   public PriorityStatus Priority { get; }

   public void AddItem(ToDoItem newItem)
   {
      Guard.Against.Null(newItem, nameof(newItem));
      this.items.Add(newItem);

      var newItemAddedEvent = new NewItemAddedEvent(this, newItem);
      this.RegisterDomainEvent(newItemAddedEvent);
   }

   public void UpdateName(string newName)
   {
      this.Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
   }
}
