using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Validator for CreateUserRequest that defines validation rules for user creation.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be valid format (using EmailValidator)
    /// - Username: Required, length between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be Unknown
    /// - Role: Cannot be None
    /// - Name: Must not be null and must contain both first and last names
    /// - Address: Must not be null and must contain valid city, street, and zipcode
    /// </remarks>
    public CreateUserRequestValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Username).NotEmpty().Length(3, 50);
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