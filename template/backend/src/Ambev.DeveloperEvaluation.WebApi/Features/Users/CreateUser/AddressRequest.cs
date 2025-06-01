namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Represents the address details of a user.
/// </summary>
public class AddressRequest
{
    /// <summary>
    /// Gets or sets the city of the address.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the street of the address.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of the address.
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// Gets or sets the zipcode of the address.
    /// </summary>
    public string Zipcode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the geolocation of the address.
    /// </summary>
    public GeolocationRequest Geolocation { get; set; } = new();
}