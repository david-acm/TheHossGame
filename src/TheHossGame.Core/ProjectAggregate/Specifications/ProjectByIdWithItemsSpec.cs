// 🃏 The HossGame 🃏
// <copyright file="ProjectByIdWithItemsSpec.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.ProjectAggregate.Specifications;

using Ardalis.Specification;
using TheHossGame.Core.ProjectAggregate;

public class ProjectByIdWithItemsSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectByIdWithItemsSpec(int projectId)
    {
        this.Query
            .Where(project => project.Id == projectId)
            .Include(project => project.Items);
    }
}
