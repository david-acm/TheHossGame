// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyTokenService.cs" company="Microsoft">
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

public class MyTokenService : RefreshTokenService<TokenRequest, TokenResponse>
{
    public MyTokenService(IConfiguration config)
    {
        this.Setup(o =>
        {
            o.TokenSigningKey =
                "TokenSigningKeyTokenSigningKeyTokenSigningKeyTokenSigningKeyTokenSigningKeyTokenSigningKey";
            o.AccessTokenValidity = TimeSpan.FromMinutes(5);
            o.RefreshTokenValidity = TimeSpan.FromHours(4);

            o.Endpoint("/refresh-token", ep => { ep.Summary(s => s.Summary = "this is the refresh token endpoint"); });
        });
    }

    public override async Task PersistTokenAsync(TokenResponse response)
    {
        await RefreshTokenRepo.StoreToken(response);

        // this method will be called whenever a new access/refresh token pair is being generated.
        // store the tokens and expiry dates however you wish for the purpose of verifying
        // future refresh requests.        
    }

    public override async Task RefreshRequestValidationAsync(TokenRequest req)
    {
        if (!await RefreshTokenRepo.TokenIsValid(req.UserId, req.RefreshToken))
            this.AddError(r => r.RefreshToken, "Refresh token is invalid!");

        // validate the incoming refresh request by checking the token and expiry against the
        // previously stored data. if the token is not valid and a new token pair should
        // not be created, simply add validation errors using the AddError() method.
        // the failures you add will be sent to the requesting client. if no failures are added,
        // validation passes and a new token pair will be created and sent to the client.        
    }

    public override Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        privileges.Roles.Add("Manager");
        privileges.Claims.Add(new("ManagerID", request.UserId));
        privileges.Permissions.Add("Manage_Department");

        // specify the user privileges to be embedded in the jwt when a refresh request is
        // received and validation has passed. this only applies to renewal/refresh requests
        // received to the refresh endpoint and not the initial jwt creation. 
        return Task.CompletedTask;
    }
}