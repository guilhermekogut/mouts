using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers
{
    public class ListUsersQueryCommand : IRequest<ListUsersResult>
    {
        /// <summary>
        /// The page number to retrieve (1-based).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of users to retrieve per page.
        /// </summary>
        public int Size { get; set; } = 10;

        /// <summary>
        /// The ordering criteria (e.g., "username asc", "email desc").
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// Filter by email address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Filter by phone number.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Filter by user status (e.g., "active", "inactive").
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Filter by user role.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Filter by user's first name.
        /// </summary>
        public string? NameFirstname { get; set; }

        /// <summary>
        /// Filter by user's last name.
        /// </summary>
        public string? NameLastname { get; set; }

        /// <summary>
        /// Filter by city in the user's address.
        /// </summary>
        public string? AddressCity { get; set; }

        /// <summary>
        /// Filter by street in the user's address.
        /// </summary>
        public string? AddressStreet { get; set; }

        /// <summary>
        /// Filter by zipcode in the user's address.
        /// </summary>
        public string? AddressZipcode { get; set; }

        /// <summary>
        /// Filter by latitude in the user's address geolocation.
        /// </summary>
        public string? AddressGeolocationLat { get; set; }

        /// <summary>
        /// Filter by longitude in the user's address geolocation.
        /// </summary>
        public string? AddressGeolocationLong { get; set; }

        /// <summary>
        /// Filter by address number.
        /// </summary>
        public int? AddressNumber { get; set; }

        /// <summary>
        /// Filter by minimum address number.
        /// </summary>
        public int? MinAddressNumber { get; set; }

        /// <summary>
        /// Filter by maximum address number.
        /// </summary>
        public int? MaxAddressNumber { get; set; }
    }
}
