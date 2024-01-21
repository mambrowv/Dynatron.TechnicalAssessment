using FluentValidation;

namespace Dynatron.Api.Controllers.Commands
{
    public record CustomerCommand(string? FirstName, string? LastName, string EmailAddress);

    public class CustomerCommandValidator : AbstractValidator<CustomerCommand>
    {
        public CustomerCommandValidator()
        {
            RuleFor(x => x.FirstName).MaximumLength(50);
            RuleFor(x => x.LastName).MaximumLength(50);
            RuleFor(x => x.EmailAddress).EmailAddress().MaximumLength(128);
        }
    }
}
