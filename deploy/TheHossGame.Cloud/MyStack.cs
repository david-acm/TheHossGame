// 🃏 The HossGame 🃏
// <copyright file="MyStack.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud
{
   using Ardalis.GuardClauses;
   using Pulumi;
   using Pulumi.AzureNative.Resources;
   using Pulumi.AzureNative.Web;

   public class MyStack : Stack
   {
      private const string WebAppName = "todo-list";
      private const string GroupName = "resourceGroup";

      public MyStack(IServicePlanBuilder servicePlanBuilder, IWebAppBuilder webAppBuilder)
      {
         Guard.Against.Null(servicePlanBuilder, nameof(servicePlanBuilder));
         Guard.Against.Null(webAppBuilder, nameof(webAppBuilder));

         // Create an Azure Resource Group
         var resourceGroup = new ResourceGroup(GroupName);

         var appServicePlan = servicePlanBuilder.BuildAppServicePlan(resourceGroup);
         var webApp = webAppBuilder.BuildWebApp(GroupName, WebAppName, appServicePlan);

         this.PublishingUserName = GetWebAppPublishingKeys(resourceGroup, webApp);
         this.ResourceGroupName = resourceGroup.Name.Apply(resource => resource);
      }

      [Output]
      public Output<string> PublishingUserName { get; set; }

      [Output]
      public Output<string> ResourceGroupName { get; set; }

      private static Output<string> GetWebAppPublishingKeys(ResourceGroup resourceGroup, WebApp webApp)
      {
         return ListWebAppPublishingCredentials.Invoke(new ListWebAppPublishingCredentialsInvokeArgs
         {
            ResourceGroupName = resourceGroup.Name,
            Name = webApp.Name,
         }).Apply(webApp => webApp.PublishingUserName);
      }
   }
}
