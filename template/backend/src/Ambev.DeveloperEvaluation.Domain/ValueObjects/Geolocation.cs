namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class Geolocation
{
    public Geolocation() { }

    public Geolocation(string lat, string @long)
    {
        Lat = lat;
        Long = @long;
    }

    /// <summary>
    /// Gets the latitude
    /// </summary>
    public string Lat { get; set; } = string.Empty;
    /// <summary>
    /// Gets the longitude
    /// </summary>
    public string Long { get; set; } = string.Empty;
}

