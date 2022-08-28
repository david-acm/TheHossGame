// 🃏 The HossGame 🃏
// <copyright file="EfRepositoryUpdate.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.IntegrationTests.Data;

using Microsoft.EntityFrameworkCore;
using TheHossGame.Core.ProjectAggregate;
using Xunit;

public class EfRepositoryUpdate : BaseEfRepoTestFixture
{
    [Fact]
    public async Task UpdatesItemAfterAddingIt()
    {
        // add a project
        var repository = this.Repository;
        var initialName = Guid.NewGuid().ToString();
        var project = new Project(initialName, PriorityStatus.Backlog);

        await repository.AddAsync(project);

        // detach the item so we get a different instance
        this.DbContext.Entry(project).State = EntityState.Detached;

        // fetch the item and update its title
        var newProject = (await repository.ListAsync())
            .FirstOrDefault(project => project.Name == initialName);
        if (newProject == null)
        {
            Assert.NotNull(newProject);
            return;
        }

        Assert.NotSame(project, newProject);
        var newName = Guid.NewGuid().ToString();
        newProject.UpdateName(newName);

        // Update the item
        await repository.UpdateAsync(newProject);

        // Fetch the updated item
        var updatedItem = (await repository.ListAsync())
            .FirstOrDefault(project => project.Name == newName);

        Assert.NotNull(updatedItem);
        Assert.NotEqual(project.Name, updatedItem?.Name);
        Assert.Equal(project.Priority, updatedItem?.Priority);
        Assert.Equal(newProject.Id, updatedItem?.Id);
    }
}
