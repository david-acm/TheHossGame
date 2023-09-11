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

using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TheHossGame.Web;
using Xunit;
using Xunit.Abstractions;

[Collection("Sequential")]
public class RegisterShould : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
    private readonly ITestOutputHelper outputHelper;

    public RegisterShould(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
    }

    [Theory]
    [CommandApiData]
    public async Task RegisterANewPlayer(CommandApiFactory apiClientFactory)
    {
        var apiClient = apiClientFactory.CreateClient();
        var tokenResponse = await apiClient.GetStringAsync("/login");

        dynamic jsonObject = JObject.Parse(tokenResponse);

        var authorizationHeader = $"{jsonObject.accessToken}";
        this.outputHelper.WriteLine($"Using authentication header: {authorizationHeader}");

        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authorizationHeader);

        var result = await apiClient
            .PostAsJsonAsync<object>(Routes.Register, default!);

        var body = await result.Content.ReadAsStringAsync();
        try
        {
            result.IsSuccessStatusCode.Should().BeTrue();
        }
        catch (Exception)
        {
            this.outputHelper.WriteLine($"Response status code was: {result.StatusCode}");
            throw;
        }
        finally
        {
            this.outputHelper.WriteLine($"Response body was: {body}");
        }

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