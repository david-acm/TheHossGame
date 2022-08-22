// 🃏 The HossGame 🃏
// <copyright file="List.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.SharedKernel.Interfaces;

public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ProjectListResponse>
{
    private readonly IReadRepository<Project> repository;

    public List(IReadRepository<Project> repository)
    {
        this.repository = repository;
    }

    [HttpGet("/Projects")]
    [SwaggerOperation(
        Summary = "Gets a list of all Projects",
        Description = "Gets a list of all Projects",
        OperationId = "Project.List",
        Tags = new[] { "ProjectEndpoints" })
    ]
    public override async Task<ActionResult<ProjectListResponse>> HandleAsync(
      CancellationToken cancellationToken = new())
    {
        var projects = await this.repository.ListAsync(cancellationToken);
        var response = new ProjectListResponse(projects
            .Select(project => new ProjectRecord(project.Id, project.Name))
            .ToList())
        {
        };

        return this.Ok(response);
    }
}
