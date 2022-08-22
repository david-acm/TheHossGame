// 🃏 The HossGame 🃏
// <copyright file="HomeController.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheHossGame.Web.ViewModels;

/// <summary>
/// A sample MVC controller that uses views.
/// Razor Pages provides a better way to manage view-based content, since the behavior, view model, and view are all in one place,
/// rather than spread between 3 different folders in your Web project. Look in /Pages to see examples.
/// See: https://ardalis.com/aspnet-core-razor-pages-%E2%80%93-worth-checking-out/
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}
