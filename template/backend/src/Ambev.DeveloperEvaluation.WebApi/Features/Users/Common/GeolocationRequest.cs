namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

/// <summary>
/// Represents the geolocation details of an address.
/// </summary>
public class GeolocationRequest
{
    /// <summary>
    /// Gets or sets the latitude of the address.
    /// </summary>
    public string Lat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the longitude of the address.
    /// </summary>
    public string Long { get; set; } = string.Empty;
}
