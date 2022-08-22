// 🃏 The HossGame 🃏
// <copyright file="ToDoItemSearchService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Services;

using Ardalis.Result;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Core.ProjectAggregate.Specifications;
using TheHossGame.SharedKernel.Interfaces;

public class ToDoItemSearchService : IToDoItemSearchService
{
    private readonly IRepository<Project> repository;

    public ToDoItemSearchService(IRepository<Project> repository)
    {
        this.repository = repository;
    }

    public async Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(int projectId, string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            var errors = new List<ValidationError>
      {
        new () { Identifier = nameof(searchString), ErrorMessage = $"{nameof(searchString)} is required." },
      };

            return Result<List<ToDoItem>>.Invalid(errors);
        }

        var projectSpec = new ProjectByIdWithItemsSpec(projectId);
        var project = await this.repository.FirstOrDefaultAsync(projectSpec);

        // TO DO: Optionally use Ardalis.GuardClauses Guard.Against.NotFound and catch
        if (project == null)
        {
            return Result<List<ToDoItem>>.NotFound();
        }

        var incompleteSpec = new IncompleteItemsSearchSpec(searchString);
        try
        {
            var items = incompleteSpec.Evaluate(project.Items).ToList();

            return new Result<List<ToDoItem>>(items);
        }
        catch (InvalidOperationException ex)
        {
            // TO DO: Log details here
            return Result<List<ToDoItem>>.Error(new[] { ex.Message });
        }
    }

    public async Task<Result<ToDoItem>> GetNextIncompleteItemAsync(int projectId)
    {
        var projectSpec = new ProjectByIdWithItemsSpec(projectId);
        var project = await this.repository.FirstOrDefaultAsync(projectSpec);
        if (project == null)
        {
            return Result<ToDoItem>.NotFound();
        }

        var incompleteSpec = new IncompleteItemsSpec();
        var items = incompleteSpec.Evaluate(project.Items).ToList();
        if (!items.Any())
        {
            return Result<ToDoItem>.NotFound();
        }

        return new Result<ToDoItem>(items.First());
    }
}
