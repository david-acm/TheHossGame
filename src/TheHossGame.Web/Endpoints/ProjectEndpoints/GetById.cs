// 🃏 The HossGame 🃏
// <copyright file="GetById.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using TheHossGame.SharedKernel.Interfaces;

public class GetById : EndpointBaseAsync
  .WithRequest<GetProjectByIdRequest>
  .WithActionResult<GetProjectByIdResponse>
{
    private readonly IRepository<Project> repository;

    public GetById(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    [HttpGet(GetProjectByIdRequest.Route)]
    [SwaggerOperation(
      Summary = "Gets a single Project",
      Description = "Gets a single Project by Id",
      OperationId = "Projects.GetById",
      Tags = new[] { "ProjectEndpoints" })
    ]
    public override async Task<ActionResult<GetProjectByIdResponse>> HandleAsync(
      [FromRoute] GetProjectByIdRequest request,
      CancellationToken cancellationToken = new())
    {
        Guard.Against.Null(request);
        var spec = new ProjectByIdWithItemsSpec(request.ProjectId);
        var entity = await this.repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (entity == null)
        {
            return this.NotFound();
        }

        var response = new GetProjectByIdResponse(
                id: entity.Id,
                name: entity.Name,
                items: entity.Items.Select(item => new ToDoItemRecord(item.Id, item.Title, item.Description, item.IsDone))
                .ToList());

        return this.Ok(response);
    }
}
