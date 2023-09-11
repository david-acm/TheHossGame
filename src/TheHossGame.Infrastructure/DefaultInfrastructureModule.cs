// 🃏 The HossGame 🃏
// <copyright file="DefaultInfrastructureModule.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace Hoss.Infrastructure;

using System.Reflection;
using Autofac;
using Hoss.Core.GameAggregate;
using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;
using MediatR;
using MediatR.Pipeline;
using TheHossGame.Infrastructure;
using Module = Autofac.Module;

public class DefaultInfrastructureModule : Module
{
    private readonly List<Assembly> assemblies = new();
    private readonly bool isDevelopment;

    public DefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
    {
        this.isDevelopment = isDevelopment;
        var coreAssembly =
            Assembly.GetAssembly(typeof(Game)); // TO DO: Replace "Project" with any type from your Core project
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

#pragma warning disable S1172 // Unused method parameters should be removed
#pragma warning disable IDE0060 // Remove unused parameter
    private static void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore S1172 // Unused method parameters should be removed
    {
#pragma warning disable S3626 // Jump statements should not be redundant
        return;
#pragma warning restore S3626 // Jump statements should not be redundant
    }

    private static void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
        throw new NotImplementedException();
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
        builder
            .RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        builder
            .RegisterType(typeof(DomainEventDispatcher<IInternalEventHandler>))
            .As(typeof(IDomainEventDispatcher<Profile>))
            .InstancePerLifetimeScope();

        builder
            .RegisterType<EntityStore>()
            .As<IEntityStore>();

        // builder.Register<ServiceFactory>(context =>
        // {
        //    var c = context.Resolve<IComponentContext>();
        //
        //    return t => c.Resolve(t);
        // });

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
    }
}