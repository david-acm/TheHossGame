// 🃏 The HossGame 🃏
// <copyright file="IncompleteItemsSearchSpec.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Specifications;

using Ardalis.Specification;

public class IncompleteItemsSearchSpec : Specification<ToDoItem>
{
  public IncompleteItemsSearchSpec(string searchString)
  {
    this.Query
        .Where(item => !item.IsDone &&
        (item.Title.Contains(searchString) ||
        item.Description.Contains(searchString)));
  }
}
