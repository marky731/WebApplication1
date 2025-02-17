using FluentValidation;
using EntityLayer.Dtos;

namespace Intermediary.Validators
{
    public class UserToAddDtoValidator : AbstractValidator<UserToAddDto>
    {
        public UserToAddDtoValidator()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty()
                .WithMessage("First name is required")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("First name cannot contain only whitespace");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Surname is required")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Surname cannot contain only whitespace");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Gender is required");

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .WithMessage("Valid Role ID is required");
        }
    }
} 