// 🃏 The HossGame 🃏
// <copyright file="ApiProjectsControllerList.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.FunctionalTests.ControllerApis;

using Ardalis.GuardClauses;
using Ardalis.HttpClientTestExtensions;
using TheHossGame.Web;
using TheHossGame.Web.ApiModels;
using Xunit;

[Collection("Sequential")]
public class ProjectCreate : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient client;

  public ProjectCreate(CustomWebApplicationFactory<WebMarker> factory)
  {
    Guard.Against.Null(factory);
    this.client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneProject()
  {
    var result = await this.client.GetAndDeserializeAsync<IEnumerable<ProjectDto>>("/api/projects");

    Assert.Single(result);
    Assert.Contains(result, i => i.Name == SeedData.TestProject1.Name);
  }
}
