// 🃏 The HossGame 🃏
// <copyright file="EfRepositoryAdd.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.IntegrationTests.Data;

using TheHossGame.Core.ProjectAggregate;
using Xunit;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
    [Fact]
    public async Task AddsProjectAndSetsId()
    {
        var testProjectName = "testProject";
        var testProjectStatus = PriorityStatus.Backlog;
        var repository = this.Repository;
        var project = new Project(testProjectName, testProjectStatus);

        await repository.AddAsync(project);

        var newProject = (await repository.ListAsync())
                        .FirstOrDefault();

        Assert.Equal(testProjectName, newProject?.Name);
        Assert.Equal(testProjectStatus, newProject?.Priority);
        Assert.True((newProject?.Id ?? 0) > 0);
    }
}
