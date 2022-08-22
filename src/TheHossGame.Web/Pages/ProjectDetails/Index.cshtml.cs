// 🃏 The HossGame 🃏
// <copyright file="Index.cshtml.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Pages.ProjectDetails;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using TheHossGame.SharedKernel.Interfaces;
using TheHossGame.Web.ApiModels;

public class IndexModel : PageModel
{
    private readonly IRepository<Project> repository;

    public IndexModel(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public int ProjectId { get; set; }

    public string Message { get; set; } = string.Empty;

    public ProjectDto? Project { get; set; }

    public async Task OnGetAsync()
    {
        var projectSpec = new ProjectByIdWithItemsSpec(this.ProjectId);
        var project = await this.repository.FirstOrDefaultAsync(projectSpec);
        if (project == null)
        {
            this.Message = "No project found.";

            return;
        }

        this.Project = new ProjectDto(
            id: project.Id,
            name: project.Name,
            items: project.Items
            .Select(item => ToDoItemDto.FromToDoItem(item))
            .ToList());
    }
}
