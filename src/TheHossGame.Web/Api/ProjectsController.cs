// 🃏 The HossGame 🃏
// <copyright file="ProjectsController.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using TheHossGame.SharedKernel.Interfaces;
using TheHossGame.Web.ApiModels;

namespace TheHossGame.Web.Api;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints.
/// </summary>
public class ProjectsController : BaseApiController
{
    private readonly IRepository<Project> repository;

    public ProjectsController(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    // GET: api/Projects
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var projectDTOs = (await this.repository.ListAsync())
            .Select(project => new ProjectDto(
                id: project.Id,
                name: project.Name))
            .ToList();

        return this.Ok(projectDTOs);
    }

    // GET: api/Projects
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var projectSpec = new ProjectByIdWithItemsSpec(id);
        var project = await this.repository.FirstOrDefaultAsync(projectSpec);
        if (project == null)
        {
            return this.NotFound();
        }

        var result = new ProjectDto(
            id: project.Id,
            name: project.Name,
            items: new List<ToDoItemDto>(
                project.Items.Select(i => ToDoItemDto.FromToDoItem(i)).ToList()));

        return this.Ok(result);
    }

    // POST: api/Projects
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProjectDto request)
    {
        Guard.Against.Null(request);
        var newProject = new Project(request.Name, PriorityStatus.Backlog);

        var createdProject = await this.repository.AddAsync(newProject);

        var result = new ProjectDto(
            id: createdProject.Id,
            name: createdProject.Name);
        return this.Ok(result);
    }

    // PATCH: api/Projects/{projectId}/complete/{itemId}
    [HttpPatch("{projectId:int}/complete/{itemId}")]
    public async Task<IActionResult> Complete(int projectId, int itemId)
    {
        var projectSpec = new ProjectByIdWithItemsSpec(projectId);
        var project = await this.repository.FirstOrDefaultAsync(projectSpec);
        if (project == null)
        {
            return this.NotFound("No such project");
        }

        var toDoItem = project.Items.FirstOrDefault(item => item.Id == itemId);
        if (toDoItem == null)
        {
            return this.NotFound("No such item.");
        }

        toDoItem.MarkComplete();
        await this.repository.UpdateAsync(project);

        return this.Ok();
    }
}
