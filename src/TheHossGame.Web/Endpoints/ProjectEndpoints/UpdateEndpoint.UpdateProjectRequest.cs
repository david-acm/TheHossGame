// 🃏 The HossGame 🃏
// <copyright file="UpdateEndpoint.UpdateProjectRequest.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using System.ComponentModel.DataAnnotations;

public class UpdateProjectRequest
{
    public const string Route = "/Projects";

    [Required]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
}
