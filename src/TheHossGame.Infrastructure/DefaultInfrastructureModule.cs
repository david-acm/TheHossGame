// 🃏 The HossGame 🃏
// <copyright file="DefaultInfrastructureModule.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Infrastructure;

using System.Reflection;
using Autofac;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Infrastructure.Data;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

public class DefaultInfrastructureModule : Module
{
    private readonly bool isDevelopment;
    private readonly List<Assembly> assemblies = new List<Assembly>();

    public DefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
    {
        this.isDevelopment = isDevelopment;
        var coreAssembly =
          Assembly.GetAssembly(typeof(Project)); // TO DO: Replace "Project" with any type from your Core project
        var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));
        if (coreAssembly != null)
        {
            this.assemblies.Add(coreAssembly);
        }

        if (infrastructureAssembly != null)
        {
            this.assemblies.Add(infrastructureAssembly);
        }

        if (callingAssembly != null)
        {
            this.assemblies.Add(callingAssembly);
        }
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (this.isDevelopment)
        {
            RegisterDevelopmentOnlyDependencies(builder);
        }
        else
        {
            RegisterProductionOnlyDependencies(builder);
        }

        this.RegisterCommonDependencies(builder);
    }

    private static void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {
        // NOTE: Add any development only services here
    }

    private static void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
        // NOTE: Add any production only services here
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EfRepository<>))
          .As(typeof(IRepository<>))
          .As(typeof(IReadRepository<>))
          .InstancePerLifetimeScope();

        builder
          .RegisterType<Mediator>()
          .As<IMediator>()
          .InstancePerLifetimeScope();

        builder
          .RegisterType<DomainEventDispatcher>()
          .As<IDomainEventDispatcher>()
          .InstancePerLifetimeScope();

        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();

            return t => c.Resolve(t);
        });

        var mediatrOpenTypes = new[]
        {
          typeof(IRequestHandler<,>),
          typeof(IRequestExceptionHandler<,,>),
          typeof(IRequestExceptionAction<,>),
          typeof(INotificationHandler<>),
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
        {
            builder
              .RegisterAssemblyTypes(this.assemblies.ToArray())
              .AsClosedTypesOf(mediatrOpenType)
              .AsImplementedInterfaces();
        }

        builder.RegisterType<EmailSender>().As<IEmailSender>()
          .InstancePerLifetimeScope();
    }
}
