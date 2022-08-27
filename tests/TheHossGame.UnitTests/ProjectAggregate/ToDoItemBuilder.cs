// 🃏 The HossGame 🃏
// <copyright file="ToDoItemBuilder.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.ProjectAggregate;

using TheHossGame.Core.ProjectAggregate;

// Learn more about test builders:
// https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
public class ToDoItemBuilder
{
    private ToDoItem todo = new ();

    public ToDoItemBuilder Id(int id)
    {
        this.todo = new ToDoItem(id);
        return this;
    }

    public ToDoItemBuilder Title(string title)
    {
        this.todo.Title = title;
        return this;
    }

    public ToDoItemBuilder Description(string description)
    {
        this.todo.Description = description;
        return this;
    }

    public ToDoItemBuilder WithDefaultValues()
    {
        this.todo = new ToDoItem(1) { Title = "Test Item", Description = "Test Description" };

        return this;
    }

    public ToDoItem Build() => this.todo;
}
