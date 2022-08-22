// 🃏 The HossGame 🃏
// <copyright file="List.ProjectListResponse.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;
public class ProjectListResponse
{
    private readonly List<ProjectRecord> projects;

    public ProjectListResponse(IList<ProjectRecord> projects)
    {
        this.projects = projects.ToList();
    }

    public IReadOnlyList<ProjectRecord> Projects => this.projects.AsReadOnly();
}
