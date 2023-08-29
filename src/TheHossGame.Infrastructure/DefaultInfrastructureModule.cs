// 🃏 The HossGame 🃏
// <copyright file="DefaultInfrastructureModule.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace Hoss.Infrastructure;

using Autofac;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using Hoss.Core.Interfaces;
using Hoss.Infrastructure.Data;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;
using Module = Autofac.Module;

public class DefaultInfrastructureModule : Module
{
   private readonly bool isDevelopment;
   private readonly List<Assembly> assemblies = new ();

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
