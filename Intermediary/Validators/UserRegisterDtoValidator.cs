 using EntityLayer.Dtos;
using FluentValidation;

namespace Intermediary.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("First name is required")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("First name cannot contain only whitespace");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Surname cannot contain only whitespace");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Valid Role ID is required");
    }
}