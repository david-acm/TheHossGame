// 🃏 The HossGame 🃏
// <copyright file="Program.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web;

using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using TheHossGame.Core;
using TheHossGame.Infrastructure;
using TheHossGame.Infrastructure.Data;
using ILogger = ILogger;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");

        builder.Services.AddDbContext(connectionString);

        builder.Services.AddControllersWithViews().AddNewtonsoftJson();
        builder.Services.AddRazorPages();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.EnableAnnotations();
        });

        // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
        builder.Services.Configure<ServiceConfig>(config =>
        {
            config.Services = new List<ServiceDescriptor>(builder.Services);

            // optional - default path to view services is /listallservices - recommended to choose your own path
            config.Path = "/listservices";
        });

        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule(new DefaultCoreModule());
            containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseShowAllServicesMiddleware();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseRouting();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });

        // Seed Database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
                SeedData.Initialize(services);
            }
            catch (InvalidOperationException ex)
            {
                var logger = services.GetRequiredService<ILogger>();
                logger.LogError($"An error occurred seeding the DB. {ex.Message}");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger>();
                logger.LogError($"An error occurred seeding the DB. {ex.Message}");
                throw;
            }
        }

        app.Run();
    }
}
