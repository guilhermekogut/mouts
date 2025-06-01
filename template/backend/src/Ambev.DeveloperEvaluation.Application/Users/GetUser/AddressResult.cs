namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public class AddressResult
{

    public AddressResult()
    {
        Geolocation = new GeolocationResult();
    }

    public AddressResult(string city, string street, int number, string zipcode, GeolocationResult geolocation)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }

    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public GeolocationResult Geolocation { get; set; } = new();
}
