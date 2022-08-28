// 🃏 The HossGame 🃏
// <copyright file="Program.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;
using Pulumi.AzureNative.Web;

public class WebAppBuilder : IWebAppBuilder
{
   public WebApp BuildWebApp(string resourceGroupName, string webappName, AppServicePlan appServicePlan)
   {
      return new WebApp(webappName, new WebAppArgs
      {
         ResourceGroupName = resourceGroupName,
         Location = appServicePlan.Location,
         ServerFarmId = appServicePlan.Id,
         HttpsOnly = true,
      });
   }
}
