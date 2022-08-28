// 🃏 The HossGame 🃏
// <copyright file="MetaController.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TheHossGame.Web.Api;

public class MetaController : BaseApiController
{
    [HttpGet("/info")]
    public ActionResult<string> Info()
    {
        var assembly = typeof(WebMarker).Assembly;

        var creationDate = System.IO.File.GetCreationTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return this.Ok($"Version: {version}, Last Updated: {creationDate}");
    }
}
