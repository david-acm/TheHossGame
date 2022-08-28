// 🃏 The HossGame 🃏
// <copyright file="HomeControllerIndex.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.FunctionalTests.ControllerViews;

using Ardalis.GuardClauses;
using TheHossGame.Web;
using Xunit;

[Collection("Sequential")]
public class HomeControllerIndex : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
   private readonly HttpClient client;

   public HomeControllerIndex(CustomWebApplicationFactory<WebMarker> factory)
   {
      Guard.Against.Null(factory);
      this.client = factory.CreateClient();
   }

   [Fact]
   public async Task ReturnsViewWithCorrectMessage()
   {
      HttpResponseMessage response = await this.client.GetAsync(new Uri(this.client.BaseAddress!, "/"));
      response.EnsureSuccessStatusCode();
      string stringResponse = await response.Content.ReadAsStringAsync();

      Assert.Contains("TheHossGame.Web", stringResponse, StringComparison.InvariantCulture);
   }
}
