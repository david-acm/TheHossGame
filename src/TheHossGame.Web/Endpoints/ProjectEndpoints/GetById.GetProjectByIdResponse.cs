// 🃏 The HossGame 🃏
// <copyright file="GetById.GetProjectByIdResponse.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

public class GetProjectByIdResponse
{
    private readonly List<ToDoItemRecord> items;

    public GetProjectByIdResponse(int id, string name, IList<ToDoItemRecord> items)
    {
        this.Id = id;
        this.Name = name;
        this.items = items.ToList();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public IList<ToDoItemRecord> Items => this.items.AsReadOnly();
}
