namespace TheHossGame.FunctionalTests;

public static class Routes
{
  public const string Registries = "/registrations";

  public static string Bid(Guid gameId) =>
    $"/{gameId}/bids";

  public static string JoinRequests(Guid gameId) =>
    $"/{gameId}/joinRequests";

  public const string Games = "/games";

  public static string ReadyFlags(Guid gameId) =>
    $"/{gameId}/playerReady";

  public static string TrumpSelection(Guid gameId) =>
    $"/{gameId}/trumpSelections";

  public static string CardPlays(Guid gameId) =>
    $"/{gameId}/cardPlays";
}