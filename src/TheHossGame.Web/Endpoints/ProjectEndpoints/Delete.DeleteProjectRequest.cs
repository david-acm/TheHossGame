// 🃏 The HossGame 🃏
// <copyright file="Delete.DeleteProjectRequest.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using System.Globalization;

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

public class DeleteProjectRequest
{
    public const string Route = "/Projects/{ProjectId:int}";

    public int ProjectId { get; set; }

    public static string BuildRoute(int projectId) => Route.Replace("{ProjectId:int}", projectId.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
}
