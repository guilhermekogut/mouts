using System.Globalization;

using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Domain.Enums;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library for ListUsersHandler.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provides both valid and invalid data scenarios.
    /// </summary>
    public static class ListUsersHandlerTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid ListUsersQueryCommand instances.
        /// </summary>
        private static readonly Faker<ListUsersQueryCommand> listUsersQueryFaker = new Faker<ListUsersQueryCommand>("pt_BR")
            .RuleFor(q => q.Page, f => f.Random.Int(1, 5))
            .RuleFor(q => q.Size, f => f.Random.Int(1, 20))
            .RuleFor(q => q.Order, f => "username asc")
            .RuleFor(q => q.Email, f => f.Internet.Email())
            .RuleFor(q => q.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(q => q.Status, f => f.PickRandom<UserStatus>().ToString())
            .RuleFor(q => q.Role, f => f.PickRandom<UserRole>().ToString())
            .RuleFor(q => q.NameFirstname, f => f.Name.FirstName())
            .RuleFor(q => q.NameLastname, f => f.Name.LastName())
            .RuleFor(q => q.AddressCity, f => f.Address.City())
            .RuleFor(q => q.AddressStreet, f => f.Address.StreetName())
            .RuleFor(q => q.AddressZipcode, f => f.Address.ZipCode())
            .RuleFor(q => q.AddressGeolocationLat, f => f.Address.Latitude().ToString("F6", CultureInfo.InvariantCulture))
            .RuleFor(q => q.AddressGeolocationLong, f => f.Address.Longitude().ToString("F6", CultureInfo.InvariantCulture))
            .RuleFor(q => q.AddressNumber, f => f.Random.Int(1, 9999))
            .RuleFor(q => q.MinAddressNumber, f => f.Random.Int(1, 10))
            .RuleFor(q => q.MaxAddressNumber, f => f.Random.Int(10, 9999));

        /// <summary>
        /// Configures the Faker to generate valid ListUserItemResult instances.
        /// </summary>
        private static readonly Faker<ListUserItemResult> listUserItemFaker = new Faker<ListUserItemResult>("pt_BR")
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Name, f => new DeveloperEvaluation.Application.Users.Common.NameResult
            {
                Firstname = f.Name.FirstName(),
                Lastname = f.Name.LastName()
            })
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
            .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
            .RuleFor(u => u.Address, f => new DeveloperEvaluation.Application.Users.Common.AddressResult
            {
                City = f.Address.City(),
                Street = f.Address.StreetName(),
                Number = f.Random.Number(1, 9999),
                Zipcode = f.Address.ZipCode(),
                Geolocation = new DeveloperEvaluation.Application.Users.Common.GeolocationResult
                {
                    Lat = f.Address.Latitude().ToString("F6", CultureInfo.InvariantCulture),
                    Long = f.Address.Longitude().ToString("F6", CultureInfo.InvariantCulture)
                }
            });

        /// <summary>
        /// Configures the Faker to generate valid ListUsersResult instances.
        /// </summary>
        private static readonly Faker<ListUsersResult> listUsersResultFaker = new Faker<ListUsersResult>("pt_BR")
            .RuleFor(r => r.Items, f => listUserItemFaker.Generate(f.Random.Int(1, 5)))
            .RuleFor(r => r.TotalCount, f => f.Random.Int(1, 100))
            .RuleFor(r => r.CurrentPage, f => f.Random.Int(1, 5))
            .RuleFor(r => r.TotalPages, f => f.Random.Int(1, 10));

        /// <summary>
        /// Generates a valid ListUsersQueryCommand with randomized data.
        /// </summary>
        public static ListUsersQueryCommand GenerateValidQuery() => listUsersQueryFaker.Generate();

        /// <summary>
        /// Generates a valid ListUsersResult with randomized data.
        /// </summary>
        public static ListUsersResult GenerateValidResult() => listUsersResultFaker.Generate();
    }
}