using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());

        RuleFor(user => user.Username)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username cannot be longer than 50 characters.");

        RuleFor(user => user.Password).SetValidator(new PasswordValidator());

        RuleFor(user => user.Phone)
            .Matches(@"^\+[1-9]\d{10,14}$")
            .WithMessage("Phone number must start with '+' followed by 11-15 digits.");

        RuleFor(user => user.Status)
            .NotEqual(UserStatus.Unknown)
            .WithMessage("User status cannot be Unknown.");

        RuleFor(user => user.Role)
            .NotEqual(UserRole.None)
            .WithMessage("User role cannot be None.");


        RuleFor(user => user.Name).NotNull();

        RuleFor(user => user.Name.Firstname)
        .NotEmpty().WithMessage("First name is required.")
        .MaximumLength(50);

        RuleFor(user => user.Name.Lastname)
        .NotEmpty().WithMessage("Last name is required.")
        .MaximumLength(50);


        RuleFor(user => user.Address).NotNull();

        RuleFor(user => user.Address.City)
        .NotEmpty().WithMessage("City is required.")
        .MaximumLength(100);

        RuleFor(user => user.Address.Street)
        .NotEmpty().WithMessage("Street is required.")
        .MaximumLength(100);

        RuleFor(user => user.Address.Zipcode).SetValidator(new ZipCodeValidator());

    }
}
