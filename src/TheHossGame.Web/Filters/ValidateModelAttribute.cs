// 🃏 The HossGame 🃏
// <copyright file="ValidateModelAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web.Filters;

using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// This filter is no longer needed since [ApiController] provides this automatically for APIs.
/// Both the BaseApiController and all ApiEndpoints in this sample use [ApiController].
/// This file is included to show how and where additional custom filters would be added to your Web project.
/// </summary>
public sealed class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Guard.Against.Null(context);
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
