// 🃏 The HossGame 🃏
// <copyright file="Program.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;
using Pulumi.AzureNative.Web;

public interface IWebAppBuilder
{
   WebApp BuildWebApp(string resourceGroupName, string webappName, AppServicePlan appServicePlan);
}