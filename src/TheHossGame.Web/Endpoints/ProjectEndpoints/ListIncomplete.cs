// 🃏 The HossGame 🃏
// <copyright file="ListIncomplete.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheHossGame.Core.Interfaces;

public class ListIncomplete : EndpointBaseAsync
  .WithRequest<ListIncompleteRequest>
  .WithActionResult<ListIncompleteResponse>
{
   private readonly IToDoItemSearchService searchService;

   public ListIncomplete(IToDoItemSearchService searchService)
   {
      this.searchService = searchService;
   }

   [HttpGet("/Projects/{ProjectId}/IncompleteItems")]
   [SwaggerOperation(
     Summary = "Gets a list of a project's incomplete items",
     Description = "Gets a list of a project's incomplete items",
     OperationId = "Project.ListIncomplete",
     Tags = new[] { "ProjectEndpoints" })
   ]
   public override async Task<ActionResult<ListIncompleteResponse>> HandleAsync(
     [FromQuery] ListIncompleteRequest request,
     CancellationToken cancellationToken = new ())
   {
      Guard.Against.Null(request);
      if (request.SearchString == null)
      {
         return this.BadRequest();
      }

      var result = await this.searchService.GetAllIncompleteItemsAsync(request.ProjectId, request.SearchString);
      var response = new ListIncompleteResponse(0, new List<ToDoItemRecord>(
          result.Value.Select(
            item => new ToDoItemRecord(
              item.Id,
              item.Title,
              item.Description,
              item.IsDone))));

      if (result.Status == ResultStatus.Ok)
      {
         response.ProjectId = request.ProjectId;
      }
      else if (result.Status == ResultStatus.Invalid)
      {
         return this.BadRequest(result.ValidationErrors);
      }
      else if (result.Status == ResultStatus.NotFound)
      {
         return this.NotFound();
      }

      return this.Ok(response);
   }
}
