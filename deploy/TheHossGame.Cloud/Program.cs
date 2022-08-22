// 🃏 The HossGame 🃏
// <copyright file="Program.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Cloud
{
  using System.Threading.Tasks;
  using Pulumi;

  internal static class Program
  {
    public static Task<int> Main() => Deployment.RunAsync<MyStack>();
  }
}
