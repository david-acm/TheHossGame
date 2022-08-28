// 🃏 The HossGame 🃏
// <copyright file="ListIncomplete.ListIncompleteResponse.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

public class ListIncompleteResponse
{
    private readonly List<ToDoItemRecord> incompleteItems;

    public ListIncompleteResponse(int projectId, IList<ToDoItemRecord> incompleteItems)
    {
        this.ProjectId = projectId;
        this.incompleteItems = incompleteItems.ToList();
    }

    public int ProjectId { get; set; }

    public IReadOnlyList<ToDoItemRecord> IncompleteItems => this.incompleteItems.AsReadOnly();
}
