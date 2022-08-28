// 🃏 The HossGame 🃏
// <copyright file="Program.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud;

using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Deployment = Pulumi.Deployment;

internal static class Program
{
   public static Task<int> Main()
   {
      var builder = new HostBuilder();
      using var services = builder.BuildScope();
      return Deployment.RunAsync<MyStack>(services.ServiceProvider);
   }
}
