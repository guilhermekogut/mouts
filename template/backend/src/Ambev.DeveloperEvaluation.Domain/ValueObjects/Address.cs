
namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;


public class Address
{
    public Address()
    {
        Geolocation = new Geolocation();
    }

    public Address(string city, string street, int? number, string zipcode, Geolocation geolocation)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }
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
    public int? Number { get; set; }
    /// <summary>
    /// Gets the zipcode
    /// </summary>
    public string Zipcode { get; set; } = string.Empty;
    /// <summary>
    /// Gets the geolocation
    /// </summary>
    public Geolocation Geolocation { get; set; } = new Geolocation();
}

