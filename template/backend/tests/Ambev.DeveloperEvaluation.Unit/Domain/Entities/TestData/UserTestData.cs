using System.Globalization;

using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class UserTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Username (using internet usernames)
    /// - Password (meeting complexity requirements)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Status (Active or Suspended)
    /// - Role (Customer or Admin)
    /// - Name (with first and last names)
    /// - Address (with city, street, number, zipcode, and geolocation)
    /// </summary>
    private static readonly Faker<User> UserFaker = new Faker<User>("pt_BR")
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
        .RuleFor(u => u.Name, f => new Name
        {
            Firstname = f.Name.FirstName(),
            Lastname = f.Name.LastName()
        })
        .RuleFor(u => u.Address, f => new Address
        {
            City = f.Address.City(),
            Street = f.Address.StreetName(),
            Number = f.Random.Number(1, 9999),
            Zipcode = f.Address.ZipCode(),
            Geolocation = new Geolocation
            {
                Lat = f.Address.Latitude().ToString("F6", CultureInfo.InvariantCulture),
                Long = f.Address.Longitude().ToString("F6", CultureInfo.InvariantCulture)
            }
        });


    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static User GenerateValidUser()
    {
        return UserFaker.Generate();
    }

    /// <summary>
    /// Generates a valid email address using Faker.
    /// The generated email will:
    /// - Follow the standard email format (user@domain.com)
    /// - Have valid characters in both local and domain parts
    /// - Have a valid TLD
    /// </summary>
    /// <returns>A valid email address.</returns>
    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }

    /// <summary>
    /// Generates a valid password that meets all complexity requirements.
    /// The generated password will have:
    /// - At least 8 characters
    /// - At least one uppercase letter
    /// - At least one lowercase letter
    /// - At least one number
    /// - At least one special character
    /// </summary>
    /// <returns>A valid password meeting all complexity requirements.</returns>
    public static string GenerateValidPassword()
    {
        return $"Test@{new Faker().Random.Number(100, 999)}";
    }

    /// <summary>
    /// Generates a valid Brazilian phone number.
    /// The generated phone number will:
    /// - Start with country code (+55)
    /// - Have a valid area code (11-99)
    /// - Have 9 digits for the phone number
    /// - Follow the format: +55XXXXXXXXXXXX
    /// </summary>
    /// <returns>A valid Brazilian phone number.</returns>
    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}";
    }

    /// <summary>
    /// Generates a valid Brazilian zipcode.
    /// The generated zipcode will:
    /// - Have 8 digits for the zipcode with hyphen
    /// - Follow the format: xxxxx-xxx
    /// </summary>
    /// <returns>A valid Brazilian zipcode.</returns>
    public static string GenerateValidZipcode()
    {
        var faker = new Faker("pt_BR");
        return faker.Address.ZipCode();
    }

    /// <summary>
    /// Generates a valid username.
    /// The generated username will:
    /// - Be between 3 and 50 characters
    /// - Use internet username conventions
    /// - Contain only valid characters
    /// </summary>
    /// <returns>A valid username.</returns>
    public static string GenerateValidUsername()
    {
        return new Faker().Internet.UserName();
    }

    /// <summary>
    /// Generates an invalid email address for testing negative scenarios.
    /// The generated email will:
    /// - Not follow the standard email format
    /// - Not contain the @ symbol
    /// - Be a simple word or string
    /// This is useful for testing email validation error cases.
    /// </summary>
    /// <returns>An invalid email address.</returns>
    public static string GenerateInvalidEmail()
    {
        var faker = new Faker();
        return faker.Lorem.Word();
    }

    /// <summary>
    /// Generates an invalid password for testing negative scenarios.
    /// The generated password will:
    /// - Not meet the minimum length requirement
    /// - Not contain all required character types
    /// This is useful for testing password validation error cases.
    /// </summary>
    /// <returns>An invalid password.</returns>
    public static string GenerateInvalidPassword()
    {
        return new Faker().Lorem.Word();
    }

    /// <summary>
    /// Generates an invalid phone number for testing negative scenarios.
    /// The generated phone number will:
    /// - Not follow the Brazilian phone number format
    /// - Not have the correct length
    /// - Not start with the country code
    /// This is useful for testing phone validation error cases.
    /// </summary>
    /// <returns>An invalid phone number.</returns>
    public static string GenerateInvalidPhone()
    {
        return new Faker().Random.AlphaNumeric(5);
    }


    /// <summary>
    /// Generates an invalid zipcode for testing negative scenarios.
    /// The generated zipcode will:
    /// - Not follow the Brazilian zipcode format
    /// - Not have the correct length
    /// This is useful for testing zipcode validation error cases.
    /// </summary>
    /// <returns>An invalid zipcode.</returns>
    public static string GenerateInvalidZipcode()
    {
        return new Faker().Random.AlphaNumeric(9);
    }

    /// <summary>
    /// Generates a username that exceeds the maximum length limit.
    /// The generated username will:
    /// - Be longer than 50 characters
    /// - Contain random alphanumeric characters
    /// This is useful for testing username length validation error cases.
    /// </summary>
    /// <returns>A username that exceeds the maximum length limit.</returns>
    public static string GenerateLongUsername()
    {
        return new Faker().Random.String2(51);
    }
}
