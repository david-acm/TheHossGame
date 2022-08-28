// 🃏 The HossGame 🃏
// <copyright file="Create.CreateProjectResponse.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

public class CreateProjectResponse
{
    public CreateProjectResponse(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; }
}
