// 🃏 The HossGame 🃏
// <copyright file="Delete.cs" company="Reactive">
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

public class Delete : EndpointBaseAsync
    .WithRequest<DeleteProjectRequest>
    .WithoutResult
{
    private readonly IRepository<Project> repository;

    public Delete(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    [HttpDelete(DeleteProjectRequest.Route)]
    [SwaggerOperation(
        Summary = "Deletes a Project",
        Description = "Deletes a Project",
        OperationId = "Projects.Delete",
        Tags = new[] { "ProjectEndpoints" })
    ]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute] DeleteProjectRequest request,
        CancellationToken cancellationToken = new ())
    {
        Guard.Against.Null(request);
        var aggregateToDelete = await this.repository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (aggregateToDelete == null)
        {
            return this.NotFound();
        }

        await this.repository.DeleteAsync(aggregateToDelete, cancellationToken);

        return this.NoContent();
    }
}
