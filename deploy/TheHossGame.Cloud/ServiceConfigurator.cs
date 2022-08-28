// 🃏 The HossGame 🃏
// <copyright file="Program.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ServiceConfigurator
{
   public static IServiceScope BuildScope(this HostBuilder builder)
   {
      builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

      builder.ConfigureContainer<ContainerBuilder>(builder =>
      {
         builder.RegisterModule(new DefaultModule());
      });

      var services = builder.Build().Services.CreateScope();
      return services;
   }
}
