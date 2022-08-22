// 🃏 The HossGame 🃏
// <copyright file="ProjectController.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using TheHossGame.SharedKernel.Interfaces;
using TheHossGame.Web.ViewModels;

[Route("[controller]")]
public class ProjectController : Controller
{
    private readonly IRepository<Project> projectRepository;

    public ProjectController(IRepository<Project> projectRepository)
    {
        this.projectRepository = projectRepository;
    }

    // GET project/{projectId?}
    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> Index(int projectId = 1)
    {
        var spec = new ProjectByIdWithItemsSpec(projectId);
        var project = await this.projectRepository.FirstOrDefaultAsync(spec);
        if (project == null)
        {
            return this.NotFound();
        }

        var dto = new ProjectViewModel(project.Items
                        .Select(item => ToDoItemViewModel.FromToDoItem(item))
                        .ToList())
        {
            Id = project.Id,
            Name = project.Name,
        };
        return this.View(dto);
    }
}
