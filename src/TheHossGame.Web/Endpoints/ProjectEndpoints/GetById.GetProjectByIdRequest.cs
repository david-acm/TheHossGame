// 🃏 The HossGame 🃏
// <copyright file="GetById.GetProjectByIdRequest.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Endpoints.ProjectEndpoints;

using System.Globalization;

public class GetProjectByIdRequest
{
    public const string Route = "/Projects/{ProjectId:int}";

    public int ProjectId { get; set; }

    public static string BuildRoute(int projectId) => Route.Replace("{ProjectId:int}", projectId.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
}
