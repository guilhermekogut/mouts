namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

public class AddressResponse
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public GeolocationResponse Geolocation { get; set; } = new();
}
