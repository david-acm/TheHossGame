// 🃏 The HossGame 🃏
// <copyright file="ToDoItemDTO.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.ApiModels;

using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using TheHossGame.Core.ProjectAggregate;

// ApiModel DTOs are used by ApiController classes and are typically kept in a side-by-side folder
public class ToDoItemDto
{
    public int Id { get; set; }

    [Required]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsDone { get; private set; }

    public static ToDoItemDto FromToDoItem(ToDoItem item)
    {
        Guard.Against.Null(item);
        return new ToDoItemDto()
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsDone = item.IsDone,
        };
    }
}
