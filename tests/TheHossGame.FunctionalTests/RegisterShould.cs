// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterShould.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.FunctionalTests;

using System.Net.Http.Json;
using Ardalis.HttpClientTestExtensions;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheHossGame.Web;
using TheHossGame.Web.Endpoints;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

[Collection("Sequential")]
public class RegisterShould : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
    [Theory]
    [CommandApiData]
    public async Task RegisterANewPlayer(CommandApiFactory commandApi)
    {
        var apiClient = commandApi.CreateClient();
        var content = StringContentHelpers.FromModelAsJson(new RegisterPlayerCommand());

        var result = await apiClient.PostAsJsonAsync(Routes.Register, new RegisterPlayerCommand());

        result.IsSuccessStatusCode.Should().BeTrue();

        await Task.CompletedTask;
    }

    #region Nested type: Startup

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
    }

    #endregion
}

public static class Routes
{
    public static string Register => "/registrations";
}

public class CommandApiFactory : WebApplicationFactory<WebMarker>
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CommandApiFactory(ITestOutputHelper testOutputHelper)
    {
        this._testOutputHelper = testOutputHelper;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureLogging(loggingBuilder =>
            loggingBuilder.Services.AddLogging(b => b.AddXUnit()));

        var host = builder.Build();

        host.Start();

        return host;
    }
}

public class CommandApiDataAttribute : AutoDataAttribute
{
    public CommandApiDataAttribute()
        : base(() => new Fixture()
            .Customize(new CompositeCustomization(
                new CommandApiCustomization())))
    {
    }
}

public class CommandApiCustomization : ICustomization
{
    #region ICustomization Members

    public void Customize(IFixture fixture)
    {
        var helper = new TestOutputHelper();
        fixture.Customize<CommandApiFactory>(f => f.FromFactory(() =>
        {
            var commandApiFactory = new CommandApiFactory(helper);
            commandApiFactory.WithWebHostBuilder(b => b.UseEnvironment(""));
            return commandApiFactory;
        }));
    }

    #endregion
}