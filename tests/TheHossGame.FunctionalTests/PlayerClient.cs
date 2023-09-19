using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace TheHossGame.FunctionalTests;

public class PlayerClient
{
    public readonly HttpClient HttpClient;

    public string PlayerId { get; private set; }

    public PlayerClient(HttpClient httpClient, string playerId)
    {
        HttpClient = httpClient;
        PlayerId = playerId;
    }

    public async Task AuthorizeAsync()
    {
        
        var tokenResponse = await HttpClient.GetStringAsync("/login");

        dynamic jsonObject = JObject.Parse(tokenResponse);

        var authorizationHeader = $"{jsonObject.accessToken}";

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authorizationHeader);
    }
}