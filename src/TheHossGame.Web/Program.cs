using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventStore.Client;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Serilog;
using TheHossGame.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


builder.Services.AddFastEndpoints();
builder.Services.AddJWTBearerAuth(
    "TokenSigningKeyTokenSigningKeyTokenSigningKeyTokenSigningKeyTokenSigningKeyTokenSigningKey");
//builder.Services.AddFastEndpointsApiExplorer();
builder.Services.SwaggerDocument(o => { o.ShortSchemaNames = true; });


// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
});

var settings = EventStoreClientSettings
    .Create("esdb://localhost:2113?tls=false");
var client = new EventStoreClient(settings);

// var eventData = new EventData(
//     Uuid.NewUuid(),
//     "some-event",
//     Encoding.UTF8.GetBytes("{\"id\": \"1\" \"value\": \"some value\"}")
// );t
//
// await client.AppendToStreamAsync(
//     "some-stream",
//     StreamState.NoStream,
//     new List<EventData> {
//         eventData
//     });
//
builder.Services.AddSingleton(client);

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // containerBuilder.RegisterModule(new DefaultCoreModule());
    containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.IsDevelopment()));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
    app.UseDefaultExceptionHandler(); // from FastEndpoints
    app.UseHsts();
}

app.UseFastEndpoints();
app.UseAuthentication(); //add this
app.UseAuthorization();
app.UseSwaggerGen(); // FastEndpoints middleware

app.UseHttpsRedirection();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

// Seed Database


app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
namespace TheHossGame.Web
{
    public partial class Program
    {
    }
}