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

using Hoss.Core.ProfileAggregate;
using Hoss.SharedKernel.Interfaces;

namespace TheHossGame.Web.Endpoints;

using FastEndpoints;
using FluentValidation;

public class RegistrationHandler : Endpoint<RegisterPlayerCommand>
{
    private readonly IAggregateStore entityStore;

    public RegistrationHandler(IAggregateStore entityStore)
    {
        this.entityStore = entityStore;
    }

    public override void Configure()
    {
        Post("/registrations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        RegisterPlayerCommand request,
        CancellationToken _)
    {
        var newProfile = Profile.FromNewRegister(
            ProfileEmail.FromString(request.Email),
            PlayerName.FromString(request.Name),
            new AProfileId(request.PlayerId));
        await entityStore.SaveAsync(
            newProfile);
    }
}

public record RegisterPlayerResponse(Profile Profile);

public record RegisterPlayerCommand([property: FromClaim] string Name, [property: FromClaim] string Email)
{
    public Guid PlayerId { get; set; }
}

public class RegisterCommandValidator : Validator<RegisterPlayerCommand>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RegisterCommandValidator" /> class.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(r => r.Email)
            .EmailAddress()
            .WithMessage("Invalid Email Address");

        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least two characters long")
            .MaximumLength(40)
            .WithMessage("Name cannot be longer that 40 characters");
    }
}