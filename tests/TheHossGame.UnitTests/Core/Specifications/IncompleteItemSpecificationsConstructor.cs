// 🃏 The HossGame 🃏
// <copyright file="IncompleteItemSpecificationsConstructor.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Specifications;

using AutoFixture.Xunit2;
using FluentAssertions;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using Xunit;

public class IncompleteItemsSpecificationConstructor
{
   [Theory]
   [AutoData]
   public void FilterCollectionToOnlyReturnItemsWithIsDoneFalse(ToDoItem item1, ToDoItem item2, ToDoItem item3)
   {
      item3.MarkComplete();

      var items = new List<ToDoItem>() { item1, item2, item3 };

      var spec = new IncompleteItemsSpec();

      var filteredList = spec.Evaluate(items);

      filteredList.Should().Contain(item1);
      filteredList.Should().Contain(item2);
      filteredList.Should().NotContain(item3);
   }
}
