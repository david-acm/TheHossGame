// 🃏 The HossGame 🃏
// <copyright file="UpdateEndpoint.cs" company="Reactive">
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

public class UpdateEndpoint : EndpointBaseAsync
    .WithRequest<UpdateProjectRequest>
    .WithActionResult<UpdateProjectResponse>
{
    private readonly IRepository<Project> repository;

    public UpdateEndpoint(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    [HttpPut(UpdateProjectRequest.Route)]
    [SwaggerOperation(
        Summary = "Updates a Project",
        Description = "Updates a Project with a longer description",
        OperationId = "Projects.Update",
        Tags = new[] { "ProjectEndpoints" })
    ]
    public override async Task<ActionResult<UpdateProjectResponse>> HandleAsync(
        UpdateProjectRequest request,
        CancellationToken cancellationToken = new ())
    {
        Guard.Against.Null(request);
        if (request.Name == null)
        {
            return this.BadRequest();
        }

        var existingProject = await this.repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingProject == null)
        {
            return this.NotFound();
        }

        existingProject.UpdateName(request.Name);

        await this.repository.UpdateAsync(existingProject, cancellationToken);

        var response = new UpdateProjectResponse(
            project: new ProjectRecord(existingProject.Id, existingProject.Name));

        return this.Ok(response);
    }
}
