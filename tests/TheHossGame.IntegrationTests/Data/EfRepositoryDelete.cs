// 🃏 The HossGame 🃏
// <copyright file="EfRepositoryDelete.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.IntegrationTests.Data;

using TheHossGame.Core.ProjectAggregate;
using Xunit;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
    [Fact]
    public async Task DeletesItemAfterAddingIt()
    {
        // add a project
        var repository = this.Repository;
        var initialName = Guid.NewGuid().ToString();
        var project = new Project(initialName, PriorityStatus.Backlog);
        await repository.AddAsync(project);

        // delete the item
        await repository.DeleteAsync(project);

        // verify it's no longer there
        Assert.DoesNotContain(
            await repository.ListAsync(),
            project => project.Name == initialName);
    }
}
