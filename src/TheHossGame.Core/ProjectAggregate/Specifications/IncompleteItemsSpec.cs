// 🃏 The HossGame 🃏
// <copyright file="IncompleteItemsSpec.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Specifications;

using Ardalis.Specification;

public class IncompleteItemsSpec : Specification<ToDoItem>
{
    public IncompleteItemsSpec()
    {
        this.Query.Where(item => !item.IsDone);
    }
}
