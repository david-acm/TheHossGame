// 🃏 The HossGame 🃏
// <copyright file="MetaControllerInfo.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.FunctionalTests.ControllerApis;

using Ardalis.GuardClauses;
using TheHossGame.Web;
using Xunit;

[Collection("Sequential")]
public class MetaControllerInfo : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient client;

  public MetaControllerInfo(CustomWebApplicationFactory<WebMarker> factory)
  {
    Guard.Against.Null(factory);
    this.client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsVersionAndLastUpdateDate()
  {
    var response = await this.client.GetAsync(new Uri("/info"));
    response.EnsureSuccessStatusCode();
    var stringResponse = await response.Content.ReadAsStringAsync();

    AssertContainsInvariantCulture("Version", stringResponse);
    AssertContainsInvariantCulture("Last Updated", stringResponse);
  }

  internal static void AssertContainsInvariantCulture(string expected, string actual)
  {
    Assert.Contains(expected, actual, StringComparison.InvariantCulture);
  }
}
