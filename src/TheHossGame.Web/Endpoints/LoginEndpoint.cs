// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginEndpoint.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.Web.Endpoints;

using FastEndpoints;
using FastEndpoints.Security;

public class LoginEndpoint : EndpointWithoutRequest<TokenResponse>
{
    public override void Configure()
    {
        this.Get("/login");
        this.AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        //user credential checking has been omitted for brevity

        this.Response = await this.CreateTokenWith<MyTokenService>("user-id-001", u =>
        {
            // u.Roles.AddRange(new[] { "Admin", "Manager" });
            // u.Permissions.Add("Update_Something");
            u.Claims.Add(new("Name", "David"));
            u.Claims.Add(new("Email", "david@email"));
        });
    }
}