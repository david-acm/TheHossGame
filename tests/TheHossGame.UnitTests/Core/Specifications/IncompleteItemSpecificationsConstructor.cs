// 🃏 The HossGame 🃏
// <copyright file="IncompleteItemSpecificationsConstructor.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Specifications;

using FluentAssertions;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using Xunit;

public class IncompleteItemsSpecificationConstructor
{
   [Fact]
   public void FilterCollectionToOnlyReturnItemsWithIsDoneFalse()
   {
      var item1 = new ToDoItem(1);
      var item2 = new ToDoItem(2);
      var item3 = new ToDoItem(3);
      item3.MarkComplete();

      var items = new List<ToDoItem> { item1, item2, item3 };

      var spec = new IncompleteItemsSpec();

      var filteredList = spec.Evaluate(items);

      var toDoItems = filteredList.ToList();
      toDoItems.Should().Contain(item1);
      toDoItems.Should().Contain(item2);
      toDoItems.Should().NotContain(item3);
   }
}
