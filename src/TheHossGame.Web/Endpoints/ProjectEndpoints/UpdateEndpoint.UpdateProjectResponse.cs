// 🃏 The HossGame 🃏
// <copyright file="UpdateEndpoint.UpdateProjectResponse.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

public class UpdateProjectResponse
{
    public UpdateProjectResponse(ProjectRecord project)
    {
        this.Project = project;
    }

    public ProjectRecord Project { get; set; }
}
