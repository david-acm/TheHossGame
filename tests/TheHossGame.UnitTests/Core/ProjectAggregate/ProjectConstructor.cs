// 🃏 The HossGame 🃏
// <copyright file="ProjectConstructor.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.ProjectAggregate;

using TheHossGame.Core.ProjectAggregate;
using Xunit;

public class ProjectConstructor
{
    private readonly string testName = "test name";
    private readonly PriorityStatus testPriority = PriorityStatus.Backlog;
    private Project? testProject;

    [Fact]
    public void InitializesName()
    {
        this.testProject = this.CreateProject();

        Assert.Equal(this.testName, this.testProject.Name);
    }

    [Fact]
    public void InitializesPriority()
    {
        this.testProject = this.CreateProject();

        Assert.Equal(this.testPriority, this.testProject.Priority);
    }

    [Fact]
    public void InitializesTaskListToEmptyList()
    {
        this.testProject = this.CreateProject();

        Assert.NotNull(this.testProject.Items);
    }

    [Fact]
    public void InitializesStatusToInProgress()
    {
        this.testProject = this.CreateProject();

        Assert.Equal(ProjectStatus.Complete, this.testProject.Status);
    }

    private Project CreateProject()
    {
        return new Project(this.testName, this.testPriority);
    }
}
