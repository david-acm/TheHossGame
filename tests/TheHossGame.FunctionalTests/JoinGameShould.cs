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
  public async Task SaveJoinEventAsync(List<PlayerClient> players, Guid gameId)
  {
    await players[0].CreateGameAsync(gameId);

    foreach (var client in players.Skip(1).Take(1))
    {
      await client.JoinPlayerToTeamAsync(gameId, NorthSouth, OutputHelper);
    }

    foreach (var client in players.Skip(2).Take(2))
    {
      await client.JoinPlayerToTeamAsync(gameId, EastWest, OutputHelper);
    }

    foreach (var client in players)
    {
      await client.ReadyAsync(gameId);
    }

    await players[0].BidAsync(gameId, 1);
    await players[2].BidAsync(gameId, 2);
    await players[1].BidAsync(gameId, 3);
    await players[3].BidAsync(gameId, 4);

    await players[3].SelectTrumpAsync(gameId, '♣');

    await players[3].PlayCardAsync(gameId, "J", '♠');
  }
}

public static class ApiClientExtensions
{
  public static async Task PlayCardAsync(this PlayerClient player, Guid gameId, string rank, char suit)
  {
    var result = await player.HttpClient
      .PostAsJsonAsync(
        Routes.CardPlays(gameId),
        new
        {
          PlayerId = player.PlayerId,
          Rank = rank,
          Suit = suit
        });
    
    await EnsureOkStatusAsync(result);
  }
  
  public static async Task SelectTrumpAsync(this PlayerClient player, Guid gameId, char suit)
  {
    var result = await player.HttpClient
      .PostAsJsonAsync(
        Routes.TrumpSelection(gameId),
        new
        {
          PlayerId = player.PlayerId,
          TrumpSuit = suit
        });
    
    await EnsureOkStatusAsync(result);
  }
  
  public static async Task BidAsync(this PlayerClient client, Guid gameId, int bidValue)
  {
    
    var result = await client.HttpClient
      .PostAsJsonAsync(
        Routes.Bid(gameId),
        new
        {
          PlayerId = client.PlayerId,
          BidValue = bidValue
        });
    
    await EnsureOkStatusAsync(result);
  }
  
  public static async Task CreateGameAsync(this PlayerClient clients, Guid gameId)
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
  }

  public static async Task JoinPlayerToTeamAsync(this PlayerClient client, Guid gameId, TeamId teamId,
    ITestOutputHelper testOutputHelper)
  {
    var result = await client.HttpClient
      .PutAsJsonAsync(
        Routes.JoinRequests(gameId),
        new
        {
          PlayerId = client.PlayerId,
          TeamId = teamId
        });

    result.IsSuccessStatusCode.Should().BeTrue();

    testOutputHelper.WriteLine(
      $"👉 Player {client.PlayerId} joined the game {gameId}");
  }

  public static async Task ReadyAsync(this PlayerClient client, Guid gameId)
  {
    var result = await client.HttpClient
      .PutAsJsonAsync(
        Routes.ReadyFlags(gameId),
        new
        {
          PlayerId = client.PlayerId
        });

    await EnsureOkStatusAsync(result);
  }

  private static async Task EnsureOkStatusAsync(HttpResponseMessage result)
  {
    result.IsSuccessStatusCode.Should().BeTrue($"Status code was {result.StatusCode} {await result.Content.ReadAsStringAsync()}");
  }
}