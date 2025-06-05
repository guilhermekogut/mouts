using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserCommand that defines validation rules for user creation command.
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateUserCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be in valid format (using EmailValidator)
    /// - Username: Required, must be between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be set to Unknown
    /// - Role: Cannot be set to None
    /// - Name: Must not be null and must contain valid first and last names
    /// - Address: Must not be null and must contain valid city, street, and zipcode
    /// </remarks>
    public UpdateUserCommandValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Password).SetValidator(new PasswordValidator());
        RuleFor(user => user.Phone).Matches(@"^\+?[1-9]\d{1,14}$");
        RuleFor(user => user.Status).NotEqual(UserStatus.Unknown);
        RuleFor(user => user.Role).NotEqual(UserRole.None);
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