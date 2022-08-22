// 🃏 The HossGame 🃏
// <copyright file="Create.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.SharedKernel.Interfaces;

public class Create : EndpointBaseAsync
  .WithRequest<CreateProjectRequest>
  .WithActionResult<CreateProjectResponse>
{
    private readonly IRepository<Project> repository;

    public Create(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    [HttpPost("/Projects")]
    [SwaggerOperation(
      Summary = "Creates a new Project",
      Description = "Creates a new Project",
      OperationId = "Project.Create",
      Tags = new[] { "ProjectEndpoints" })
    ]
    public override async Task<ActionResult<CreateProjectResponse>> HandleAsync(
      CreateProjectRequest request,
      CancellationToken cancellationToken = new())
    {
        Guard.Against.Null(request);
        if (request.Name == null)
        {
            return this.BadRequest();
        }

        var newProject = new Project(request.Name, PriorityStatus.Backlog);
        var createdItem = await this.repository.AddAsync(newProject, cancellationToken);
        var response = new CreateProjectResponse(
            id: createdItem.Id,
            name: createdItem.Name);

        return this.Ok(response);
    }
}
