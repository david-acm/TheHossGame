// 🃏 The HossGame 🃏
// <copyright file="ProjectList.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.FunctionalTests.ApiEndpoints;

using Ardalis.GuardClauses;
using Ardalis.HttpClientTestExtensions;
using TheHossGame.Web;
using TheHossGame.Web.Endpoints.ProjectEndpoints;
using Xunit;

[Collection("Sequential")]
public class ProjectList : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient client;

  public ProjectList(CustomWebApplicationFactory<WebMarker> factory)
  {
    Guard.Against.Null(factory);
    this.client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneProject()
  {
    var result = await this.client.GetAndDeserializeAsync<ProjectListResponse>("/Projects");

    Assert.Single(result.Projects);
    Assert.Contains(result.Projects, i => i.name == SeedData.TestProject1.Name);
  }
}
