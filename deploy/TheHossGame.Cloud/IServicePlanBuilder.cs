// 🃏 The HossGame 🃏
// <copyright file="IServicePlanBuilder.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;

public interface IServicePlanBuilder
{
    AppServicePlan BuildAppServicePlan(ResourceGroup resourceGroup);
}
