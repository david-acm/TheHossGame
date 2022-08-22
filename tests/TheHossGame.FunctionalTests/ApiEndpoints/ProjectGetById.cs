// 🃏 The HossGame 🃏
// <copyright file="ProjectGetById.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.FunctionalTests.ApiEndpoints;

using System.Net;
using Ardalis.GuardClauses;
using Ardalis.HttpClientTestExtensions;
using TheHossGame.Web;
using TheHossGame.Web.Endpoints.ProjectEndpoints;
using Xunit;

[Collection("Sequential")]
public class ProjectGetById : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient client;

  public ProjectGetById(CustomWebApplicationFactory<WebMarker> factory)
  {
    Guard.Against.Null(factory);
    this.client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsSeedProjectGivenId1()
  {
    var result = await this.client.GetAndDeserializeAsync<GetProjectByIdResponse>(GetProjectByIdRequest.BuildRoute(1));

    Assert.Equal(1, result.Id);
    Assert.Equal(SeedData.TestProject1.Name, result.Name);
    Assert.Equal(3, result.Items.Count);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    string route = GetProjectByIdRequest.BuildRoute(0);
    var response = await this.client.GetAndEnsureNotFoundAsync(route);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}
