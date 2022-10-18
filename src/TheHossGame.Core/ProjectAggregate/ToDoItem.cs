﻿// 🃏 The HossGame 🃏
// <copyright file="ToDoItem.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate;

using TheHossGame.Core.ProjectAggregate.Events;
using TheHossGame.SharedKernel;

public class ToDoItem : EntityBase
{
   public ToDoItem(int id)
     : base(id)
   {
   }

   public ToDoItem()
   {
   }

   public string Title { get; set; } = string.Empty;

   public string Description { get; set; } = string.Empty;

   public bool IsDone { get; private set; }

   protected override bool IsNull => false;

   public void MarkComplete()
   {
      if (this.IsDone)
      {
         return;
      }

      this.IsDone = true;

      this.RaiseDomainEvent(new ToDoItemCompletedEvent(this));
   }

   public override string ToString()
   {
      string status = this.IsDone ? "Done!" : "Not done.";
      return $"{this.Id}: Status: {status} - {this.Title} - {this.Description}";
   }

   protected override void When(DomainEventBase @event)
   {
      throw new NotImplementedException();
   }

   protected override void EnsureValidState()
   {
      throw new NotImplementedException();
   }
}
