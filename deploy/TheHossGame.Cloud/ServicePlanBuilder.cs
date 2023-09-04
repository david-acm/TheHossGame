// 🃏 The HossGame 🃏
// <copyright file="ServicePlanBuilder.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;

using Ardalis.GuardClauses;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

public class ServicePlanBuilder : IServicePlanBuilder
{
    public ServicePlanBuilder()
    {
    }

    #region IServicePlanBuilder Members

    public AppServicePlan BuildAppServicePlan(ResourceGroup resourceGroup)
    {
        Guard.Against.Null(resourceGroup, nameof(resourceGroup));
        return new AppServicePlan("asp", new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Kind = "App",
            Sku = new SkuDescriptionArgs
            {
                Capacity = 1,
                Family = "P",
                Name = "P1",
                Size = "P1",
                Tier = "Premium",
            },
        });
    }

    #endregion
}