// 🃏 The HossGame 🃏
// <copyright file="ToDoItemRecord.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

public record ToDoItemRecord(int id, string title, string description, bool isDone);
