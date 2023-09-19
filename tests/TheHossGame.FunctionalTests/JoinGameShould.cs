using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Hoss.Core.GameAggregate;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TheHossGame.Web;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using static Hoss.Core.GameAggregate.TeamId;

namespace TheHossGame.FunctionalTests;

public abstract class FunctionalTest : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private HttpClient? apiClient;
  protected readonly ITestOutputHelper OutputHelper;

  protected FunctionalTest(ITestOutputHelper outputHelper)
  {
    OutputHelper = outputHelper;
  }

  protected async Task<PlayerClient> GetAuthorizedClientAsync(CommandApiFactory apiClientFactory)
  {
    // TODO: Make this an auto fixture customization. 
    apiClient = apiClientFactory.CreateClient();
    var tokenResponse = await apiClient.GetStringAsync("/login");

    dynamic jsonObject = JObject.Parse(tokenResponse);

    var authorizationHeader = $"{jsonObject.accessToken}";
    OutputHelper.WriteLine($"Using authentication header: {authorizationHeader}");

    apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authorizationHeader);

    var json = new
    {
      PlayerId = Guid.NewGuid().ToString()
    };

    var result = await apiClient
      .PostAsJsonAsync<object>(Routes.Registries, json);

    return new PlayerClient(apiClient, json.PlayerId);
  }

  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
    }
  }
}

[Startup(startupType: typeof(Startup))]
public class JoinGameShould : FunctionalTest
{
  public JoinGameShould(ITestOutputHelper outputHelper)
    : base(outputHelper)
  {
  }

  [Theory]
  [PlayerRegisteredClientData]
  public async Task SaveJoinEventAsync(List<PlayerClient> clients, Guid gameId)
  {
    await clients[0].CreateGameAsync(gameId);

    foreach (var client in clients.Skip(1).Take(1))
    {
      await client.JoinPlayerToTeamAsync(gameId, NorthSouth, OutputHelper);
    }

    foreach (var client in clients.Skip(2).Take(2))
    {
      await client.JoinPlayerToTeamAsync(gameId, EastWest, OutputHelper);
    }
    
    
  }
}

public static class ApiClientExtensions
{
  public static async Task<HttpResponseMessage> CreateGameAsync(this PlayerClient clients, Guid gameId)
  {
    var result = await clients.HttpClient
      .PostAsJsonAsync(
        Routes.Games,
        new
        {
          PlayerId = clients.PlayerId,
          GameId = gameId,
        });
    
    result.IsSuccessStatusCode.Should().BeTrue();
    
    return result;
  }

  public static async Task JoinPlayerToTeamAsync(this PlayerClient client, Guid gameId, TeamId teamId,
    ITestOutputHelper testOutputHelper)
  {
    var result = await client.HttpClient
      .PutAsJsonAsync(
        Routes.JoinRequests,
        new
        {
          PlayerId = client.PlayerId,
          Id = gameId,
          TeamId = teamId
        });

    result.IsSuccessStatusCode.Should().BeTrue();

    testOutputHelper.WriteLine(
      $"👉 Player {client.PlayerId} joined the game {gameId}");
  }
}