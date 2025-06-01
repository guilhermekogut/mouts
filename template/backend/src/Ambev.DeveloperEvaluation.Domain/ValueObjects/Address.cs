
namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;


public class Address
{
    /// <summary>
    /// Gets the city name
    /// </summary>
    public string City { get; set; } = string.Empty;
    /// <summary>
    /// Gets the street name
    /// </summary>
    public string Street { get; set; } = string.Empty;
    /// <summary>
    /// Gets the number in the street
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// Gets the zipcode
    /// </summary>
    public string Zipcode { get; set; } = string.Empty;
    /// <summary>
    /// Gets the geolocation
    /// </summary>
    public Geolocation Geolocation { get; set; } = new Geolocation();
}

