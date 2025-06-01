namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public class GeolocationResult
{

    public GeolocationResult() { }

    public GeolocationResult(string lat, string @long)
    {
        Lat = lat;
        Long = @long;
    }


    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}
