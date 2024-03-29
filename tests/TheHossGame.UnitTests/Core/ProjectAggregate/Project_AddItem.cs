﻿// 🃏 The HossGame 🃏
// <copyright file="Project_AddItem.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.ProjectAggregate;

using TheHossGame.Core.ProjectAggregate;
using Xunit;

public class ProjectAddItem
{
   private readonly Project testProject = new ("some name", PriorityStatus.Backlog);

   [Fact]
   public void AddsItemToItems()
   {
      var testItem = new ToDoItem
      {
         Title = "title",
         Description = "description",
      };

      this.testProject.AddItem(testItem);

      Assert.Contains(testItem, this.testProject.Items);
   }

   [Fact]
   public void ThrowsExceptionGivenNullItem()
   {
      void Action() => this.testProject.AddItem(null!);

      var ex = Assert.Throws<ArgumentNullException>(Action);
      Assert.Equal("newItem", ex.ParamName);
   }
}
