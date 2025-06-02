using System.Globalization;

using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Enums;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library for GetUserHandler.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetUserHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetUserCommand instances.
    /// </summary>
    private static readonly Faker<GetUserCommand> getUserHandlerFaker = new Faker<GetUserCommand>("pt_BR")
        .CustomInstantiator(f => new GetUserCommand(f.Random.Guid()));

    /// <summary>
    /// Configures the Faker to generate valid GetUserResult instances.
    /// </summary>
    private static readonly Faker<GetUserResult> getUserResultFaker = new Faker<GetUserResult>("pt_BR")
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => new NameResult
        {
            Firstname = f.Name.FirstName(),
            Lastname = f.Name.LastName()
        })
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
        .RuleFor(u => u.Address, f => new AddressResult
        {
            City = f.Address.City(),
            Street = f.Address.StreetName(),
            Number = f.Random.Number(1, 9999),
            Zipcode = f.Address.ZipCode(),
            Geolocation = new GeolocationResult
            {
                Lat = f.Address.Latitude().ToString("F6", CultureInfo.InvariantCulture),
                Long = f.Address.Longitude().ToString("F6", CultureInfo.InvariantCulture)
            }
        });

    /// <summary>
    /// Generates a valid GetUserCommand with randomized data.
    /// </summary>
    public static GetUserCommand GenerateValidCommand()
    {
        return getUserHandlerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetUserResult with randomized data.
    /// </summary>
    public static GetUserResult GenerateValidResult()
    {
        return getUserResultFaker.Generate();
    }
}