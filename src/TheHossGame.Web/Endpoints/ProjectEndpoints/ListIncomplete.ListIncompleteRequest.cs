// 🃏 The HossGame 🃏
// <copyright file="ListIncomplete.ListIncompleteRequest.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using Microsoft.AspNetCore.Mvc;

public class ListIncompleteRequest
{
    [FromRoute]
    public int ProjectId { get; set; }

    [FromQuery]
    public string? SearchString { get; set; }
}
