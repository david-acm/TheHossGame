// 🃏 The HossGame 🃏
// <copyright file="DefaultCoreModule.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core;

using Autofac;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.Services;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
