// 🃏 The HossGame 🃏
// <copyright file="DefaultModule.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;

using Autofac;

public class DefaultModule : Module
{
   protected override void Load(ContainerBuilder builder)
   {
      builder.RegisterType<MyStack>()
            .As<MyStack>().InstancePerLifetimeScope();
      builder.RegisterType<ServicePlanBuilder>()
         .As<IServicePlanBuilder>();
      builder.RegisterType<WebAppBuilder>()
         .As<IWebAppBuilder>();
   }
}
