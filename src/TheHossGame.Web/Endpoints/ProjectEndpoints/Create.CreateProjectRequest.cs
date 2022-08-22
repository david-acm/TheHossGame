// 🃏 The HossGame 🃏
// <copyright file="Create.CreateProjectRequest.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using System.ComponentModel.DataAnnotations;

public class CreateProjectRequest
{
    public const string Route = "/Projects";

    [Required]
    public string? Name { get; set; }
}
