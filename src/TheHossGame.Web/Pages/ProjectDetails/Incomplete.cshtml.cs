// 🃏 The HossGame 🃏
// <copyright file="Incomplete.cshtml.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Pages.ProjectDetails;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using TheHossGame.SharedKernel.Interfaces;

public class IncompleteModel : PageModel
{
    private readonly IRepository<Project> repository;

    public IncompleteModel(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public int ProjectId { get; set; }

    public IList<ToDoItem>? ToDoItems { get; private set; }

    public async Task OnGetAsync()
    {
        var projectSpec = new ProjectByIdWithItemsSpec(this.ProjectId);
        var project = await this.repository.FirstOrDefaultAsync(projectSpec);

        if (project == null)
        {
            return;
        }

        var spec = new IncompleteItemsSpec();

        this.ToDoItems = spec.Evaluate(project.Items).ToList();
    }
}
