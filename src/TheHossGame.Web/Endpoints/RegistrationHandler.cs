// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationHandler.cs" company="Microsoft">
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
using FluentValidation;
using Hoss.Core.PlayerAggregate;
using TheHossGame.Infrastructure;

public class RegistrationHandler : Endpoint<RegisterPlayerCommand, RegisterPlayerResponse>
{
    private readonly IEntityStore entityStore;

    public RegistrationHandler(IEntityStore entityStore)
    {
        this.entityStore = entityStore;
    }

    public override void Configure()
    {
        this.Post("/registrations");
        this.AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterPlayerCommand request, CancellationToken _)
    {
        await this.entityStore.SaveAsync(
            Profile.FromNewRegister(
                ProfileEmail.FromString(request.Email),
                PlayerName.FromString(request.Name)));

        await this.SendAsync(new RegisterPlayerResponse(request));
    }
}

public record RegisterPlayerResponse(RegisterPlayerCommand command);

public record RegisterPlayerCommand([property: FromClaim] string Name, [property: FromClaim] string Email);

public class RegisterCommandValidator : Validator<RegisterPlayerCommand>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RegisterCommandValidator" /> class.
    /// </summary>
    public RegisterCommandValidator()
    {
        this.RuleFor(r => r.Email)
            .EmailAddress()
            .WithMessage("Invalid Email Address");

        this.RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least two characters long")
            .MaximumLength(40)
            .WithMessage("Name cannot be longer that 40 characters");
    }
}